using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Services.Database;
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

        services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.WriteIndented = true;
        });

        services.AddDbContext<RegisterDbContext>(
            options =>
            {
                SqlServerDbContextOptionsExtensions.UseSqlServer(options, Environment.GetEnvironmentVariable("MDDBConnString"));
            });


        var apiUrl = Environment.GetEnvironmentVariable("APIMgmtURL");
        var options = new ApiOptions { ApiUrl = apiUrl! };

        services.AddSingleton(options);
        services.AddScoped<IRegisterDb, RegisterDb>();
        RegisterUseCases(services);
        services.AddHttpClient("APIMgmt", client =>
        {
            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("APIMgmtURL")!);
        });

    })

    .Build();


host.Run();

static void RegisterUseCases(IServiceCollection services)
{
    services.AddScoped<IGetOrganisationByNumberUseCase, GetOrganisationByNumberUseCase>();
    services.AddScoped<IGetOrganisationsListUseCase, GetOrganisationsListUseCase>();

    services.AddScoped<IGetQualificationByNumberUseCase, GetQualificationByNumberUseCase>();
    services.AddScoped<IGetQualificationsListUseCase, GetQualificationsUseCase>();
}
