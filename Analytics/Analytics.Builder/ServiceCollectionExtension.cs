using Analytics.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Builder;

internal static class ServiceCollectionExtension
{
    public static void AddAnalytics(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAnalyticsData(configuration);
    }
}