using Authorization.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authorization.Infrastructure;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthorization(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.ConfigureDataLayer(configuration);
    }

    private static void ConfigureDataLayer(this IApplicationBuilder app, IConfiguration configuration)
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