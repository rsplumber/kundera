using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Seeders;

public class SeedBackgroundWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedBackgroundWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Seeding started.");
        var scope = _serviceProvider.CreateScope();
        var dataMigratorService = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await dataMigratorService.SeedAsync();
        Console.WriteLine("Seeding done!");
    }
}