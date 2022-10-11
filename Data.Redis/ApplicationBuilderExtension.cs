using Data.Redis.Permissions;
using Data.Redis.Roles;
using Data.Redis.Scopes;
using Data.Redis.Services;
using Data.Redis.UserGroups;
using Data.Redis.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureDataRedis(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return;
        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            new List<Type>
            {
                typeof(UserGroupDataModel), typeof(UserDataModel), typeof(RoleDataModel),
                typeof(PermissionDataModel), typeof(ScopeDataModel), typeof(ServiceDataModel)
            }.ForEach(type =>
            {
                if (dbProvider.Connection.GetIndexInfo(type) is null)
                {
                    dbProvider.Connection.CreateIndex(type);
                }
            });
            var seed = serviceScope.ServiceProvider.GetRequiredService<DefaultDataSeeder>();
            seed.SeedAsync().Wait();
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}