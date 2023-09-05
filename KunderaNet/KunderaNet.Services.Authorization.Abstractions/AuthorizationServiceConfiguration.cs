using Microsoft.Extensions.DependencyInjection;

namespace KunderaNet.Services.Authorization.Abstractions;

public sealed class AuthorizationServiceConfiguration
{
    public IServiceCollection ServiceCollection { get; private init; }

    public AuthorizationServiceConfiguration(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }
}