using Authentication.Data.Redis;
using Authorization.Data.Redis;
using Microsoft.AspNetCore.Builder;

namespace Authentication.Infrastructure;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureAuth(this IApplicationBuilder app)
    {
        app.ConfigureAuthenticationDataRedis();
        app.ConfigureAuthorizationDataRedis();
    }
}