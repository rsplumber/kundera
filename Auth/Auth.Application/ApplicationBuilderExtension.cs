using Auth.Application.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Auth.Application;

internal static class ApplicationBuilderExtension
{
    public static void UseAuth(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return;
        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionAuthProviderWrapper>();
            if (dbProvider.Connection.GetIndexInfo(typeof(CredentialDataModel)) is null)
            {
                dbProvider.Connection.CreateIndex(typeof(CredentialDataModel));
            }

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