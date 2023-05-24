using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Data.Auth;
using Data.Auth.Credentials;
using Data.Auth.Sessions;
using Data.Groups;
using Data.Permissions;
using Data.Roles;
using Data.Scopes;
using Data.Services;
using Data.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Data;

public static class ServiceCollectionExtension
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ??
                               throw new ArgumentNullException("connectionString", "Enter connection string in app settings");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.TryAddScoped<IUserRepository, UserRepository>();
        services.TryAddScoped<IGroupRepository, GroupRepository>();

        services.TryAddScoped<IServiceRepository, ServiceRepository>();
        services.TryAddScoped<IScopeRepository, ScopeRepository>();
        services.TryAddScoped<IRoleRepository, RoleRepository>();
        services.TryAddScoped<IPermissionRepository, PermissionRepository>();

        services.TryAddScoped<ICredentialRepository, CredentialRepository>();
        services.TryAddScoped<IAuthenticationActivityRepository, AuthenticationActivityRepository>();
        services.TryAddScoped<ISessionRepository, SessionRepository>();
        services.TryAddScoped<IAuthorizationActivityRepository, AuthorizationActivityRepository>();

        services.TryAddScoped<IAuthorizeDataProvider, AuthorizeDataProvider>();
        services.AddDistributedMemoryCache();
    }
}