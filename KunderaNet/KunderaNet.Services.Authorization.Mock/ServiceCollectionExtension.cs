using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KunderaNet.Services.Authorization.Mock;

public static class ServiceCollectionExtension
{
    public static void AddKunderaMockService(this IServiceCollection services, IConfiguration? configuration = null)
    {
        services.AddSingleton<IAuthorizeService, AuthorizeService>();
    }
}