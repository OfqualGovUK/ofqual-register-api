using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using System.Text.Json;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("RedisConnString");
        });

        RegisterUseCases(services);
        RegisterRepositories(services);

        services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = true;
        });
    })
    //.ConfigureLogging((HostBuilderContext hostingContext, ILoggingBuilder logging)=>{
    //    logging.AddConsole();
    //    logging.AddDebug().SetMinimumLevel(LogLevel.Debug);
    //})

    .Build();


host.Run();

static void RegisterUseCases(IServiceCollection services)
{
    services.AddScoped<IGetOrganisationByNumberUseCase, GetOrganisationByNumberUseCase>();
    services.AddScoped<IGetOrganisationsUseCase, GetOrganisationsUseCase>();

    services.AddScoped<IGetQualificationByNumberUseCase, GetQualificationByNumberUseCase>();
    services.AddScoped<IGetQualificationsUseCase, GetQualificationsUseCase>();
}

static void RegisterRepositories(IServiceCollection services)
{
    var _connectionString = Environment.GetEnvironmentVariable("MDDBConnString")!;
    services.AddSingleton<IRedisCacheService, RedisCache>();
    services.AddTransient<IRegisterRepository, RegisterRepository>(sp =>
    {
        var connectionString = _connectionString;
        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        return new RegisterRepository(connectionString, loggerFactory);
    });
}
