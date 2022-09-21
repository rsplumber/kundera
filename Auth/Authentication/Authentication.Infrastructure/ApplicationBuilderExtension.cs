using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infrastructure;

public static class ApplicationBuilderExtension
{
    public static void ConfigureAuthenticationService(this IApplicationBuilder app, IConfiguration configuration)
    {
    }
}