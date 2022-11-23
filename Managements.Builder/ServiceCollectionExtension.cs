using Managements.Domain.Groups;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Managements.Builder;

public static class ServiceCollectionExtension
{
    public static void AddManagements(this IServiceCollection services)
    {
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IScopeFactory, ScopeFactory>();
        services.AddScoped<IRoleFactory, RoleFactory>();
        services.AddScoped<IPermissionFactory, PermissionFactory>();
        services.AddScoped<IGroupFactory, GroupFactory>();
    }
}