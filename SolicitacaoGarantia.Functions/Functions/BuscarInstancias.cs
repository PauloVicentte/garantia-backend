using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SolicitacaoGarantia_Functions.Services;

namespace SolicitacaoGarantia.Functions.Functions;

public class BuscarInstancias
{
    private readonly CamundaClient _camunda;

    public BuscarInstancias(CamundaClient camunda)
    {
        _camunda = camunda;
    }

    [Function("BuscarInstancias")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "processos/todas")] HttpRequestData req)
    {
        var instancias = await _camunda.ListarInstanciasAsync();
        var resultado = new List<object>();

        foreach (var inst in instancias)
        {
            var id = inst.GetProperty("id").GetString();
            if (id != null)
            {
                var vars = await _camunda.ObterVariaveisAsync(id);
                resultado.Add(new { id, vars });
            }
        }

        var res = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await res.WriteAsJsonAsync(resultado);
        return res;
    }
}
