using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolicitacaoGarantia.Functions.Models;
using SolicitacaoGarantia_Functions.Helpers;
using SolicitacaoGarantia_Functions.Services;
using System.Net;
using System.Text.Json;

namespace SolicitacaoGarantia.Functions.Functions;

public class CriarSolicitacaoFunction
{
    private readonly CamundaClient _camunda;
    private readonly ILogger<CriarSolicitacaoFunction> _logger;

    public CriarSolicitacaoFunction(CamundaClient camunda, ILogger<CriarSolicitacaoFunction> logger)
    {
        _camunda = camunda;
        _logger = logger;
    }

    [Function("CriarSolicitacao")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "solicitacao")] HttpRequestData req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var solicitacao = JsonSerializer.Deserialize<Solicitacao>(body);

            if (solicitacao is null)
                return await req.CreateErrorResponseAsync(HttpStatusCode.BadRequest, "Solicitação inválida.");

            var variaveis = new
            {
                beneficioSolicitado = new
                {
                    value = JsonSerializer.Serialize(solicitacao),
                    type = "Json"
                }
            };

            var processId = await _camunda.IniciarProcessoAsync("SolicitacaoGarantiaAparelhos", variaveis);
            return await req.CreateJsonResponseAsync(HttpStatusCode.OK, new { processId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar solicitação");
            return await req.CreateErrorResponseAsync(HttpStatusCode.InternalServerError, "Erro ao criar solicitação.");
        }
    }
}
