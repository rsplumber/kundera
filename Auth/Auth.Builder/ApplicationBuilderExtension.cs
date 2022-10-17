using Auth.Data.Redis;
using Microsoft.AspNetCore.Builder;

namespace Auth.Builder;

internal static class ApplicationBuilderExtension
{
    public static void ConfigureAuth(this IApplicationBuilder app)
    {
        app.ConfigureAuthData();
    }
}