using System.Reflection;
using Core.Domains.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Sessions;
using Core.Domains.Users;
using Core.Services;
using Managements.Data.ConnectionProviders;
using Managements.Data.Credentials;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Sessions;
using Managements.Data.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Managements.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionUrl = configuration.GetSection("RedisConnections:Managements").Value;
        if (connectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Managements db connection in appsettings.json");
        }

        services.TryAddSingleton(_ => new RedisConnectionManagementsProviderWrapper(connectionUrl));

        var authConnectionUrl = configuration.GetSection("RedisConnections:Auth").Value;
        if (authConnectionUrl is null)
        {
            throw new ArgumentException("Enter RedisConnections:Auth db connection in appsettings.json");
        }

        services.TryAddSingleton(_ => new RedisConnectionAuthProviderWrapper(authConnectionUrl));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IAuthorizeService, AuthorizeService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return Assembly.GetExecutingAssembly();
    }
}