using System.Reflection;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Users;
using Managements.Domain.Groups;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Managements.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddManagementsData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Managements")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return Assembly.GetExecutingAssembly();
    }
}