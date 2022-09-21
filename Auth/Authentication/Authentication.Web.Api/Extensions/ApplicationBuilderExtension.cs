using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Authentication.Web.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthenticationWeb(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
    }
}