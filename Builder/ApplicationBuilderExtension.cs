using Auth.Builder;
using Data.Seeder;
using Managements.Builder;
using Microsoft.AspNetCore.Builder;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        app.UseModules();
        app.UseDataSeeder();
    }

    private static void UseModules(this IApplicationBuilder app)
    {
        app.UseAuth();
        app.UseManagements();
    }
}