using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Users.Web.Api;

public static class ApplicationBuilderExtension
{
    public static void ConfigureUsersWeb(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
    }
}