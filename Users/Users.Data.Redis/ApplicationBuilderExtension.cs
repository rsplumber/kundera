using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Users.Data.Redis.UserGroups;
using Users.Data.Redis.Users;

namespace Users.Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureDataLayer(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return;
        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            if (dbProvider.Connection.GetIndexInfo(typeof(UserGroupDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(UserGroupDataModel));
            }

            if (dbProvider.Connection.GetIndexInfo(typeof(UserDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(UserDataModel));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}