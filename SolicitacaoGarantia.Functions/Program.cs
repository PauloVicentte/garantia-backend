using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolicitacaoGarantia.Functions.Services;
using SolicitacaoGarantia_Functions.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddHttpClient<CamundaClient>();
        services.AddSingleton<DatabaseService>();
    })
    .Build();

host.Run();