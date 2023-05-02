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

namespace Data;

public static class ServiceCollectionExtension
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ??
                               throw new ArgumentNullException("connectionString", "Enter connection string in app settings");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        // services.AddScoped<AuthorizeDataProvider>();
        services.AddScoped<IAuthorizeDataProvider, AuthorizeDataProvider>();
        services.AddDistributedMemoryCache();
    }
}