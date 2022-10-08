using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authentication.Data.Redis;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureAuthenticationDataRedis(this IApplicationBuilder app)
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