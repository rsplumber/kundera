using Authentication.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authentication.Infrastructure;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthenticationService(this IApplicationBuilder app, IConfiguration configuration)
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
            if (dbProvider.Connection.GetIndexInfo(typeof(CredentialDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(CredentialDataModel));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}