using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Auth.Data;

internal static class ApplicationBuilderExtension
{
    public static Assembly ConfigureAuthData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return Assembly.GetExecutingAssembly();
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

        return Assembly.GetExecutingAssembly();
    }
}