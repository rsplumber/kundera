using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Authorization.Web.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthorizationWeb(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
    }
}