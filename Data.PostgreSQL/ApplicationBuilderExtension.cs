using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ApplicationBuilderExtension
{
    public static void UseData(this IApplicationBuilder app)
    {
            var dbProvider = app.ApplicationServices.GetRequiredService<AppDbContext>();
            dbProvider.Database.Migrate();
    }
}