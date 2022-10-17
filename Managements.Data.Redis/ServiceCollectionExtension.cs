using System.Reflection;
using Managements.Data.Redis.Permissions;
using Managements.Data.Redis.Roles;
using Managements.Data.Redis.Scopes;
using Managements.Data.Redis.Services;
using Managements.Data.Redis.UserGroups;
using Managements.Data.Redis.Users;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.UserGroups;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Managements.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static Assembly AddManagementsData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddTransient<DefaultDataSeeder>();

        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Managements")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return Assembly.GetExecutingAssembly();
    }
}