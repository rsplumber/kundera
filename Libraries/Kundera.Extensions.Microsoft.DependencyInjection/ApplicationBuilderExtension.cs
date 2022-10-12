using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Kundera.Extensions.Microsoft.DependencyInjection;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app, IConfiguration configuration)
    {
    }
}