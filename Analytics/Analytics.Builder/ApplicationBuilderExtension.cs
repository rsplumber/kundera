using Analytics.Data;
using Microsoft.AspNetCore.Builder;

namespace Analytics.Builder;

internal static class ApplicationBuilderExtension
{
    public static void UseAnalytics(this IApplicationBuilder app)
    {
        app.ConfigureAnalyticsData();
    }
}