using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Users.Data.Redis;

namespace Users.Builder;

public static class ApplicationBuilderExtension
{
    public static void ConfigureUsers(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.ConfigureDataLayer(configuration);
    }
}