using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Users.Builder;

public static class ApplicationBuilderExtension
{
    public static void ConfigureUsers(this IApplicationBuilder app, IConfiguration configuration)
    {
    }
}