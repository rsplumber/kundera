using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Core.Hashing;
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
        services.TryAddScoped<IPermissionFactory, PermissionFactory>();
        services.TryAddScoped<IGroupFactory, GroupFactory>();
        services.TryAddScoped<ISessionFactory, SessionFactory>();
        services.TryAddScoped<ICredentialFactory, CredentialFactory>();
    }
}