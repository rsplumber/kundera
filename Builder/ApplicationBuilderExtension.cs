using Authentication.Infrastructure;
using Data.Redis;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        app.ConfigureAuth();
        app.ConfigureDataRedis();
    }
}