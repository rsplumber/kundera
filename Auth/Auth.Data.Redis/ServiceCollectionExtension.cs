﻿using System.Reflection;
using Auth.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Redis.OM;
using StackExchange.Redis;

namespace Auth.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddAuthData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionUrl = configuration.GetSection("RedisConnections:Auth").Value;
        if (connectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Auth db connection in appsettings.json");
        }

        services.TryAddSingleton(_ => new RedisConnectionAuthProviderWrapper(connectionUrl));
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return Assembly.GetExecutingAssembly();
    }
}

internal class RedisConnectionAuthProviderWrapper : RedisConnectionProvider
{
    public RedisConnectionAuthProviderWrapper(string connectionString) : base(connectionString)
    {
    }

    public RedisConnectionAuthProviderWrapper(RedisConnectionConfiguration connectionConfig) : base(connectionConfig)
    {
    }

    public RedisConnectionAuthProviderWrapper(ConfigurationOptions configurationOptions) : base(configurationOptions)
    {
    }

    public RedisConnectionAuthProviderWrapper(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }
}