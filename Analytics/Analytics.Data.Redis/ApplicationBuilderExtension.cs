using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Analytics.Data;

internal static class ApplicationBuilderExtension
{
    public static Assembly ConfigureAnalyticsData(this IApplicationBuilder app)
    {

        return Assembly.GetExecutingAssembly();
    }
}