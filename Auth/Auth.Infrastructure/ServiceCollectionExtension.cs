﻿using Auth.Application;
using Authentication.Data.Redis;
using Authorization.Data.Redis;
using Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Standard.Tokens.JWT;

namespace Authentication.Infrastructure;

internal static class ServiceCollectionExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorizationDependencies(configuration);
        services.AddAuthenticationDependencies(configuration);
    }

    private static void AddAuthorizationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorizationDataRedis(configuration);
        services.AddTokensJwt(configuration);
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