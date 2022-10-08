using Data.Redis.Permissions;
using Data.Redis.Roles;
using Data.Redis.Scopes;
using Data.Redis.Services;
using Data.Redis.UserGroups;
using Data.Redis.Users;
using Domain.Permissions;
using Domain.Roles;
using Domain.Scopes;
using Domain.Services;
using Domain.UserGroups;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddDataRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Application")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}