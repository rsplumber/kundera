using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Authorization.Infrastructure;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthorization(this IApplicationBuilder app, IConfiguration configuration)
    {
    }
}