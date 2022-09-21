using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Web.Api;

public static class ApplicationBuilderExtension
{
    public static void ConfigureKunderaWeb(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
    }
}