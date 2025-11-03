using Npgsql;

namespace SolicitacaoGarantia.Functions.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
            ?? throw new InvalidOperationException("String de conexão não configurada.");
    }

    public async Task<NpgsqlConnection> GetOpenConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
