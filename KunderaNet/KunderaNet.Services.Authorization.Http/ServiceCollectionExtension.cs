using System.Net.Http.Headers;
using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KunderaNet.Services.Authorization.Http;

public static class ServiceCollectionExtension
{
    public static void AddKunderaHttpService(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetSection("Kundera:BaseUrl").Value;
        if (baseUrl is null)
        {
            throw new ArgumentException("Enter Kundera:BaseUrl in appsettings.json");
        }

        var serviceSecret = configuration.GetSection("Kundera:ServiceSecret").Value;

        services.AddKunderaHttpService(baseUrl, serviceSecret);
        services.TryAddSingleton<IAuthorizeService, AuthorizeService>();
    }

    public static void AddKunderaHttpService(this IServiceCollection services, string baseUrl, string? serviceSecret)
    {
        services.AddHttpClient("default", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            if (serviceSecret is not null)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("service_secret", serviceSecret);
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.TryAddSingleton<IAuthorizeService, AuthorizeService>();
    }
}