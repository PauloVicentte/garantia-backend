using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SolicitacaoGarantia_Functions.Services;

namespace SolicitacaoGarantia.Functions.Functions;

public class ExcluirInstancia
{
    private readonly CamundaClient _camunda;

    public ExcluirInstancia(CamundaClient camunda)
    {
        _camunda = camunda;
    }

    [Function("ExcluirInstancia")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "processos/{id}")] HttpRequestData req, string id)
    {
        await _camunda.ExcluirInstanciaAsync(id);
        var res = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await res.WriteAsJsonAsync(new { mensagem = "Instância excluída com sucesso.", id });
        return res;
    }
}
