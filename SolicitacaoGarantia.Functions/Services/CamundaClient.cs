using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace SolicitacaoGarantia_Functions.Services;

public class CamundaClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public CamundaClient(IConfiguration config, HttpClient http)
    {
        var raw = config["Camunda.Configuracoes"];
        var json = JsonSerializer.Deserialize<List<CamundaConfig>>(raw ?? "[]");
        _baseUrl = json?.FirstOrDefault()?.Url
            ?? throw new InvalidOperationException("Camunda URL não configurada.");
        _http = http;
    }

    public async Task<string> IniciarProcessoAsync(string processoKey, object variaveis)
    {
        var payload = new { variables = variaveis };
        var response = await _http.PostAsJsonAsync($"{_baseUrl}/process-definition/key/{processoKey}/start", payload);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Erro ao iniciar processo {processoKey}: {response.StatusCode}");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("id").GetString()!;
    }

    public async Task<List<JsonElement>> ListarInstanciasAsync()
    {
        var response = await _http.GetAsync($"{_baseUrl}/process-instance");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<JsonElement>>() ?? [];
    }

    public async Task<Dictionary<string, object>> ObterVariaveisAsync(string processInstanceId)
    {
        var response = await _http.GetAsync($"{_baseUrl}/process-instance/{processInstanceId}/variables?deserializeValues=false");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Dictionary<string, object>>() ?? [];
    }

    public async Task ExcluirInstanciaAsync(string id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/process-instance/{id}");
        response.EnsureSuccessStatusCode();
    }
}


public class CamundaConfig
{
    public string Sistema { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
