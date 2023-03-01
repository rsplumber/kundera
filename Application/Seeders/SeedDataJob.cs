using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Application.Seeders;

internal class SeedDataJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public SeedDataJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Seeding started.");
        var scope = _serviceProvider.CreateScope();
        var dataMigratorService = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await dataMigratorService.SeedAsync();
        Console.WriteLine("Seeding done!");
    }
}