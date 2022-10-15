using Auth.Application.Authentication;
using Auth.Application.Authorization;
using Authentication.Data.Redis;
using Authentication.Infrastructure.Authentication;
using Authentication.Infrastructure.Authorization;
using Authorization.Data.Redis;
using Kite.Cache.InMemory;
using Kite.Tokens.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure;

internal static class ServiceCollectionExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheInMemory();
        services.AddAuthorizationDependencies(configuration);
        services.AddAuthenticationDependencies(configuration);
    }

    private static void AddAuthorizationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorizationDataRedis(configuration);
        services.AddTokensJwt(configuration);
        services.Configure<SessionOptions>(configuration.GetSection("Sessions"));
        services.AddScoped<IAuthorizeService, AuthorizeService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ISessionManagement, SessionManagement>();
    }

    private static void AddAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationDataRedis(configuration);
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ICredentialService, CredentialService>();
    }
}