using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddAnalyticsData(this IServiceCollection services, IConfiguration configuration)
    {
        return Assembly.GetExecutingAssembly();
    }
}