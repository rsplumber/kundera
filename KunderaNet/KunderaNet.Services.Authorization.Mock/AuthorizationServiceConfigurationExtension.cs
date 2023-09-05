using KunderaNet.Services.Authorization.Abstractions;

namespace KunderaNet.Services.Authorization.Mock;

public static class AuthorizationServiceConfigurationExtension
{
    public static void UseMock(this AuthorizationServiceConfiguration configuration)
    {
        configuration.ServiceCollection.AddKunderaMockService();
    }
}