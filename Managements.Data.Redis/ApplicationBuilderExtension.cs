using System.Reflection;
using Managements.Data.Redis.Permissions;
using Managements.Data.Redis.Roles;
using Managements.Data.Redis.Scopes;
using Managements.Data.Redis.Services;
using Managements.Data.Redis.UserGroups;
using Managements.Data.Redis.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Managements.Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static Assembly ConfigureManagementsData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()
            ?.CreateScope();

        if (serviceScope is null) return Assembly.GetExecutingAssembly();

        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            new List<Type>
            {
                typeof(UserGroupDataModel),
                typeof(UserDataModel),
                typeof(RoleDataModel),
                typeof(PermissionDataModel),
                typeof(ScopeDataModel),
                typeof(ServiceDataModel)
            }.ForEach(type =>
            {
                if (dbProvider.Connection.GetIndexInfo(type) is null)
                {
                    dbProvider.Connection.CreateIndex(type);
                }
            });

            var seed = serviceScope.ServiceProvider.GetRequiredService<DefaultDataSeeder>();
            seed.SeedAsync()
                .Wait();
        }
        catch (Exception e)
        {
            // ignored
        }

        return Assembly.GetExecutingAssembly();
    }
}