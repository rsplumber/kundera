using Auth.Data;
using Microsoft.AspNetCore.Builder;

namespace Auth.Builder;

internal static class ApplicationBuilderExtension
{
    public static void UseAuth(this IApplicationBuilder app)
    {
        app.ConfigureAuthData();
    }
}