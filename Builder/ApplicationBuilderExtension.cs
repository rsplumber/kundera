using Data.Seeder;
using DotNetCore.CAP;
using Managements.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Builder;

public static class ApplicationBuilderExtension
{
    public static void UseKundera(this IApplicationBuilder app)
    {
        var eventBusBootstrapper = app.ApplicationServices.GetRequiredService<IBootstrapper>();
        eventBusBootstrapper.BootstrapAsync();
        app.UseData();
        app.UseDataSeeder();
    }
}