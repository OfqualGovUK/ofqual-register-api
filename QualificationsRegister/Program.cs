using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Data;
using Ofqual.Common.RegisterAPI.Services.Data.Repository;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        //RegisterGateways(services);
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("RedisConnString").ToString();
        });

        services.AddSingleton<IRedisCacheService, RedisCache>();
        services.AddTransient<IRegisterRepository, RegisterRepository>();
        services.AddTransient<IDapperDbConnection, DapperDbConnection>();

        RegisterUseCases(services);
    })
    .Build();


host.Run();


static void RegisterUseCases(IServiceCollection services)
{
    services.AddScoped<IGetOrganisationByReferenceUseCase, GetOrganisationByReferenceUseCase>();
    services.AddScoped<IGetOrganisationsUseCase, GetOrganisationsUseCase>();

}

/*static void RegisterGateways(IServiceCollection services)
{
    services.AddScoped<IQualificationGateway, IQualificationGateway>();
    services.AddScoped<IGetOrganisationsUseCase, IGetOrganisationsUseCase>();
}*/
