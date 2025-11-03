using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace SolicitacaoGarantia_Functions.Helpers;

public static class HttpResponse
{
    public static async Task<HttpResponseData> CreateJsonResponseAsync(this HttpRequestData req, HttpStatusCode code, object data)
    {
        var res = req.CreateResponse(code);
        await res.WriteAsJsonAsync(data);
        return res;
    }

    public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData req, HttpStatusCode code, string message)
    {
        var res = req.CreateResponse(code);
        await res.WriteAsJsonAsync(new { erro = message });
        return res;
    }
}
