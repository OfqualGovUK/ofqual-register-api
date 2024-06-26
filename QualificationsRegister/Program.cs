using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.UseCase;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using System.Text.Json;
using Ofqual.Common.RegisterAPI.Services.Database;
using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.UseCase.Qualifications;
using Ofqual.Common.RegisterFrontend.RegisterAPI;
using Refit;
using Ofqual.Common.RegisterAPI.UseCase.RecognitionScopes;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        RegisterUseCases(services);

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

        services.AddRefitClient<IRefDataAPIClient>().ConfigureHttpClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("RefDataAPIUrl")!);
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
