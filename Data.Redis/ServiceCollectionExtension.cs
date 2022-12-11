using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Core.Services;
using Managements.Data.Auth;
using Managements.Data.Auth.Credentials;
using Managements.Data.Auth.Sessions;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Redis.OM;

namespace Managements.Data;

internal static class ServiceCollectionExtension
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionUrl = configuration.GetSection("RedisConnections:Default").Value;
        if (connectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Default db connection in appsettings.json");
        }

        services.TryAddSingleton(_ => new RedisConnectionProvider(connectionUrl));

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IGroupRepository, GroupRepository>();

        services.AddSingleton<IServiceRepository, ServiceRepository>();
        services.AddSingleton<IScopeRepository, ScopeRepository>();
        services.AddSingleton<IRoleRepository, RoleRepository>();
        services.AddSingleton<IPermissionRepository, PermissionRepository>();

        services.AddSingleton<ICredentialRepository, CredentialRepository>();
        services.AddSingleton<ISessionRepository, SessionRepository>();

        services.AddSingleton<IAuthorizeService, AuthorizeService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}