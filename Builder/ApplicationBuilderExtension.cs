using Auth.Data;
using Data.Seeder;
using Managements.Data;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        app.UseAuthData();
        app.UseData();
        app.UseDataSeeder();
    }
}