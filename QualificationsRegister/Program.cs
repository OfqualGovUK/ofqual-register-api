using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("RedisConnString").ToString();
        });

        services.AddSingleton<IRedisCacheService, RedisCache>();
        services.AddTransient<IRegisterRepository, RegisterRepository>();

        //RegisterGateways(services);
        RegisterUseCases(services);
        ConfigureServices(services);
    })
    .Build();


host.Run();

static void ConfigureServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("MDDBConnString");
    services.AddDbContext<RegisterDbContext>(options => options.UseSqlServer(connectionString));
}

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
