using Auth.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth.BackgroundJobs;

public class RemoveExpiredSessionsJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private const int TenSeconds = 10 * 1000;

    public RemoveExpiredSessionsJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("RemoveExpiredSessionsJob started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceProvider.CreateScope();
            var dataMigratorService = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            await dataMigratorService.DeleteExpiredAsync(stoppingToken);
            await Task.Delay(TenSeconds, stoppingToken);
        }
    }
}