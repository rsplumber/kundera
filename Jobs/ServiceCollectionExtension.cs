using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Jobs;

public static class ServiceCollectionExtension
{
    public static void AddJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RemoveExpiredCredentialsJob>();
        services.AddScoped<RemoveExpiredSessionsJob>();
        services.AddQuartz(q =>
        {
            q.SchedulerId = "Kundera";
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 2; });

            q.ScheduleJob<RemoveExpiredCredentialsJob>(trigger => trigger
                .WithIdentity("RemoveExpiredCredentials")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(5)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
                .WithDescription("RemoveExpiredCredentials")
            );
            q.ScheduleJob<RemoveExpiredSessionsJob>(trigger => trigger
                .WithIdentity("RemoveExpiredSessionsJob")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
                .WithDescription("RemoveExpiredSessionsJob")
            );
        });
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });
    }
}