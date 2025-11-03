using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolicitacaoGarantia.Functions.Helpers;
using SolicitacaoGarantia.Functions.Models;
using SolicitacaoGarantia.Functions.Services;
using System.Net;
using System.Text.Json;
using Npgsql;
using SolicitacaoGarantia_Functions.Helpers;

namespace SolicitacaoGarantia.Functions.Functions;

public class LoginFunction
{
    private readonly ILogger<LoginFunction> _logger;
    private readonly DatabaseService _db;
    private readonly string _jwtSecret;

    public LoginFunction(ILogger<LoginFunction> logger, DatabaseService db)
    {
        _logger = logger;
        _db = db;
        _jwtSecret = Environment.GetEnvironmentVariable("JwtSecret") ?? "chave-super-secreta-trocar";
    }

    [Function("Login")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] HttpRequestData req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var loginData = JsonSerializer.Deserialize<LoginRequest>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loginData is null || string.IsNullOrWhiteSpace(loginData.Email) || string.IsNullOrWhiteSpace(loginData.Senha))
                return await req.CreateErrorResponseAsync(HttpStatusCode.BadRequest, "E-mail e senha são obrigatórios.");

            await using var connection = await _db.GetOpenConnectionAsync();

            const string query = """
                SELECT id, nome, email, cpf, data_nascimento, celular 
                FROM users 
                WHERE LOWER(email) = LOWER(@e) AND senha = @s;
            """;

            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@e", loginData.Email);
            cmd.Parameters.AddWithValue("@s", loginData.Senha);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return await req.CreateErrorResponseAsync(HttpStatusCode.Unauthorized, "E-mail ou senha incorretos.");

            var user = new
            {
                id = reader.GetInt32(0),
                nome = reader.GetString(1),
                email = reader.GetString(2),
                cpf = reader.GetString(3),
                dataNascimento = reader.GetDateTime(4).ToString("yyyy-MM-dd"),
                celular = reader.GetString(5)
            };

            var token = TokenHelper.GerarToken(user.id, user.nome, _jwtSecret);
            return await req.CreateJsonResponseAsync(HttpStatusCode.OK, new { token, user });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login");
            return await req.CreateErrorResponseAsync(HttpStatusCode.InternalServerError, "Erro interno no servidor.");
        }
    }
}
