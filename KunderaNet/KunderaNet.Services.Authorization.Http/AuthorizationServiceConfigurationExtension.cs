using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.Extensions.Configuration;

namespace KunderaNet.Services.Authorization.Http;

public static class AuthorizationServiceConfigurationExtension
{
    public static void UseHttpService(this AuthorizationServiceConfiguration serviceConfiguration, IConfiguration configuration)
    {
        serviceConfiguration.ServiceCollection.AddKunderaHttpService(configuration);
    }

    public static void UseHttpService(this AuthorizationServiceConfiguration serviceConfiguration, string baseUrl, string serviceSecret)
    {
        serviceConfiguration.ServiceCollection.AddKunderaHttpService(baseUrl, serviceSecret);
    }
}