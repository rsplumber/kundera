using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using RoleManagement.Data.Redis.Permissions;
using RoleManagement.Data.Redis.Roles;
using RoleManagement.Data.Redis.Scopes;
using RoleManagement.Data.Redis.Services;
using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Services;

namespace RoleManagement.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("RoleManagement")));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}