using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Seeder;

internal static class ApplicationBuilderExtension
{
    public static void UseDataSeeder(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

        if (serviceScope is null) return;
        try
        {
            var seed = serviceScope.ServiceProvider.GetRequiredService<DataSeeder>();
            seed.SeedAsync().Wait();
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}