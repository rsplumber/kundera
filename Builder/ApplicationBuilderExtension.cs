using Analytics.Builder;
using Managements.Builder;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        app.UseModules();
    }

    private static void UseModules(this IApplicationBuilder app)
    {
        app.UseAnalytics();
        app.UseManagements();
    }
}