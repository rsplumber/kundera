using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using RoleManagement.Data.Redis.Roles;

namespace RoleManagement.Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureDataLayer(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return;
        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            if (dbProvider.Connection.GetIndexInfo(typeof(RoleDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(RoleDataModel));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}