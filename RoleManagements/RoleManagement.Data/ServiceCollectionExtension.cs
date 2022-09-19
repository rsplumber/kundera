using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleManagement.Data.Permissions;
using RoleManagement.Data.Roles;
using RoleManagement.Data.Scopes;
using RoleManagement.Data.Services;
using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Services;

namespace RoleManagement.Data;

internal static class ServiceCollectionExtension
{
    public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
    }
}