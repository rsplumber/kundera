using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using RoleManagement.Data.Redis.Permissions;
using RoleManagement.Data.Redis.Roles;
using RoleManagement.Data.Redis.Scopes;
using RoleManagement.Data.Redis.Services;

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
            if (dbProvider.Connection.GetIndexInfo(typeof(PermissionDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(PermissionDataModel));
            }
            if (dbProvider.Connection.GetIndexInfo(typeof(ScopeDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(ScopeDataModel));
            }
            if (dbProvider.Connection.GetIndexInfo(typeof(ServiceDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(ServiceDataModel));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}