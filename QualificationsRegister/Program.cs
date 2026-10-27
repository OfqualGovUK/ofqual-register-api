using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using Ofqual.Common.RegisterAPI.Services.Database;
using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.UseCase.Qualifications;
using Ofqual.Common.RegisterFrontend.RegisterAPI;
using Refit;
using Ofqual.Common.RegisterAPI.UseCase.RecognitionScopes;
using Microsoft.Extensions.Configuration;
using Ofqual.Common.RegisterAPI.Helpers;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        ConfigurationHelper.Initialise(context.Configuration);
        
        RegisterUseCases(services);

        var connString = ConfigurationHelper.GetValue("MDDBConnString");

        services.AddDbContext<RegisterDbContext>(
            options =>
            {
                options.UseSqlServer(connString);
            });

        services.AddScoped<IRegisterDb, RegisterDb>();

        services.AddHttpClient();
        services.AddHttpClient("APIMgmt", client =>
        {
            client.BaseAddress = new Uri(ConfigurationHelper.GetValue("APIMgmtURL")!);
        });

        services.AddRefitClient<IRefDataAPIClient>().ConfigureHttpClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri(ConfigurationHelper.GetValue("RefDataAPIUrl")!);
        });
    })
    .Build();


host.Run();

static void RegisterUseCases(IServiceCollection services)
{
    services.AddScoped<IGetOrganisationByNumberUseCase, GetOrganisationByNumberUseCase>();
    services.AddScoped<IGetOrganisationsListUseCase, GetOrganisationsListUseCase>();

    services.AddScoped<IGetQualificationByNumberUseCase, GetQualificationByNumberUseCase>();
    services.AddScoped<IGetQualificationsListUseCase, GetQualificationsListUseCase>();

    services.AddScoped<IGetScopesByOrganisationNumberUseCase, GetScopesByOrganisationNumberUseCase>();
}