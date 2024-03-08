using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.UseCase;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using System.Text.Json;
using Ofqual.Common.RegisterAPI.Services.Database;
using Microsoft.EntityFrameworkCore;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        RegisterUseCases(services);

        services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = true;
        });

        services.AddDbContext<RegisterDbContext>(
            options =>
            {
                SqlServerDbContextOptionsExtensions.UseSqlServer(options, Environment.GetEnvironmentVariable("MDDBConnString"));
            });

        services.AddScoped<IRegisterDb, RegisterDb>();

        services.AddHttpClient();
        services.AddHttpClient("APIMgmt", client =>
        {
            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("APIMgmtURL")!);
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
