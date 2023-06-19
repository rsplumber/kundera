using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Core.Groups;
using Core.Hashing;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core;

public static class ServiceCollectionExtension
{
    public static void AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IHashService>(_ => new HmacHashingService(HashingType.HMACSHA384, 6));
        services.TryAddScoped<IUserFactory, UserFactory>();
        services.TryAddScoped<IServiceFactory, ServiceFactory>();
        services.TryAddScoped<IScopeFactory, ScopeFactory>();
        services.TryAddScoped<IRoleFactory, RoleFactory>();
        services.TryAddScoped<IGroupFactory, GroupFactory>();
        services.TryAddScoped<ISessionFactory, SessionFactory>();
        services.TryAddScoped<ICredentialFactory, CredentialFactory>();
        services.TryAddScoped<IPermissionService, PermissionService>();

        services.TryAddScoped<ISessionManagement, SessionManagement>();
        services.TryAddScoped<IAuthorizeService, AuthorizeService>();

        services.TryAddTransient<AuthenticatedEventHandler>();
        services.TryAddTransient<AuthorizedEventHandler>();
    }
}