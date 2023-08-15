using Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExecutionOptionsExtension
{
    public static void UseEntityFramework(this DataExecutionOptions dataExecutionOptions)
    {
        using var serviceScope = dataExecutionOptions.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        try
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}