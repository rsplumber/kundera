using System.Reflection;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Users;
using Managements.Domain.Groups;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Redis.OM;
using StackExchange.Redis;

namespace Managements.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddManagementsData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionUrl = configuration.GetSection("RedisConnections:Managements").Value;
        if (connectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Managements db connection in appsettings.json");
        }
        services.TryAddSingleton(_ => new RedisConnectionManagementsProviderWrapper(connectionUrl));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return Assembly.GetExecutingAssembly();
    }
}

internal class RedisConnectionManagementsProviderWrapper : RedisConnectionProvider
{
    public RedisConnectionManagementsProviderWrapper(string connectionString) : base(connectionString)
    {
    }

    public RedisConnectionManagementsProviderWrapper(RedisConnectionConfiguration connectionConfig) : base(connectionConfig)
    {
    }

    public RedisConnectionManagementsProviderWrapper(ConfigurationOptions configurationOptions) : base(configurationOptions)
    {
    }

    public RedisConnectionManagementsProviderWrapper(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }
}