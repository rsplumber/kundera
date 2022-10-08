using Authentication.Infrastructure;
using Data.Redis;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void ConfigureKundera(this IApplicationBuilder app)
    {
        app.ConfigureDataRedis();
        app.ConfigureAuth();
    }
}