using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authorization.Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureAuthorizationDataRedis(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return;
        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            if (dbProvider.Connection.GetIndexInfo(typeof(SessionDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(SessionDataModel));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}