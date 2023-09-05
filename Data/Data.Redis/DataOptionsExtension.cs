using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Core.Groups;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using Data.Abstractions;
using Data.Auth;
using Data.Auth.Credentials;
using Data.Auth.Sessions;
using Data.Groups;
using Data.Permissions;
using Data.Roles;
using Data.Scopes;
using Data.Services;
using Data.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Redis.OM;

namespace Data;

public static class DataOptionsExtension
{
    public static void UseRedis(this DataOptions dataOptions, IConfiguration configuration)
    {
        var connectionUrl = configuration.GetSection("RedisConnections:Default").Value;
        if (connectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Default db connection in appsettings.json");
        }

        dataOptions.Services.TryAddSingleton(_ => new RedisConnectionProvider(connectionUrl));

        dataOptions.Services.TryAddScoped<IUserRepository, UserRepository>();
        dataOptions.Services.TryAddScoped<IGroupRepository, GroupRepository>();

        dataOptions.Services.TryAddScoped<IServiceRepository, ServiceRepository>();
        dataOptions.Services.TryAddScoped<IScopeRepository, ScopeRepository>();
        dataOptions.Services.TryAddScoped<IRoleRepository, RoleRepository>();
        dataOptions.Services.TryAddScoped<IPermissionRepository, PermissionRepository>();

        dataOptions.Services.TryAddScoped<ICredentialRepository, CredentialRepository>();
        dataOptions.Services.TryAddScoped<ISessionRepository, SessionRepository>();

        dataOptions.Services.TryAddScoped<IAuthorizeDataProvider, AuthorizeDataProvider>();

        dataOptions.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}