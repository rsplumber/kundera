﻿using Authorization.Application;
using Authorization.Domain;
using Authorization.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Tes.Serializer.Microsoft;
using Tes.Standard.Tokens.JWT;

namespace Authorization.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddTokensJwt(configuration);

        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IAuthorizeService, AuthorizeService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ISessionManagement, SessionManagement>();

        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authorization")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}