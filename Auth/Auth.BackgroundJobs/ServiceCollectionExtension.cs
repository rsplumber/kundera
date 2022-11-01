using Microsoft.Extensions.DependencyInjection;

namespace Auth.BackgroundJobs;

internal static class ServiceCollectionExtension
{
    public static void AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<RemoveExpiredCredentialsJob>();
        services.AddHostedService<RemoveExpiredSessionsJob>();
    }
}