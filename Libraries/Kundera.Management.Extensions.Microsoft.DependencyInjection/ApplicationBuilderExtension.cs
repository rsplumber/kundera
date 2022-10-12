using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Kundera.Management.Extensions.Microsoft.DependencyInjection;

public static class ApplicationBuilderExtension
{
    public static void UseManagementKundera(this IApplicationBuilder app, IConfiguration configuration)
    {
    }
}