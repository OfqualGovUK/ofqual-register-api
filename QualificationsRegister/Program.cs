using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QualificationsRegister.UseCase;
using QualificationsRegister.UseCase.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        //RegisterGateways(services);
        RegisterUseCases(services);
    })
    .Build();

host.Run();

static void RegisterUseCases(IServiceCollection services)
{
    services.AddScoped<IGetOrganisationByNumberUseCase, GetOrganisationByNumberUseCase>();
    services.AddScoped<IGetOrganisationsSearchUseCase, GetOrganisationsSearchUseCase>();
}
/*static void RegisterGateways(IServiceCollection services)
{
    services.AddScoped<IQualificationGateway, IQualificationGateway>();
    services.AddScoped<IGetOrganisationsSearchUseCase, IGetOrganisationsSearchUseCase>();
}*/
