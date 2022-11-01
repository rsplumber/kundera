using Auth.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth.BackgroundJobs;

public class RemoveExpiredCredentialsJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private const int TenSeconds = 10 * 1000;

    public RemoveExpiredCredentialsJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("RemoveExpiredCredentialsJob started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceProvider.CreateScope();
            var credentialRepository = scope.ServiceProvider.GetRequiredService<ICredentialRepository>();
            await credentialRepository.DeleteExpiredAsync(stoppingToken);
            await Task.Delay(TenSeconds, stoppingToken);
        }
    }
}