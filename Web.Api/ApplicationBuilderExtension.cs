using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Web.Api;

public static class ApplicationBuilderExtension
{
    public static void UseKunderaWeb(this IApplicationBuilder app, IConfiguration? configuration = default)
    {
        app.UseRouting();
        app.UseHealthChecks("/health");
        app.UseCors(builder => builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());
    }
}