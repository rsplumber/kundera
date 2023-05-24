using System.Text.Json;
using Application.Auth;
using Application.Auth.Sessions;
using Application.Seeders;
using Core.Domains.Auth.Sessions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

namespace Application;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(options =>
        {
            options.FailedRetryCount = 2;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
            options.SucceedMessageExpiredAfter = 2;
            options.UseRabbitMQ(op =>
            {
                op.HostName = configuration.GetValue<string>("RabbitMQ:HostName") ?? throw new ArgumentNullException("RabbitMQ:HostName", "Enter RabbitMQ:HostName in app settings");
                op.UserName = configuration.GetValue<string>("RabbitMQ:UserName") ?? throw new ArgumentNullException("RabbitMQ:UserName", "Enter RabbitMQ:UserName in app settings");
                op.Password = configuration.GetValue<string>("RabbitMQ:Password") ?? throw new ArgumentNullException("RabbitMQ:Password", "Enter RabbitMQ:UserName in app settings");
                op.ExchangeName = configuration.GetValue<string>("RabbitMQ:ExchangeName") ?? throw new ArgumentNullException("RabbitMQ:ExchangeName", "Enter RabbitMQ:ExchangeName in app settings");
            });
            options.UsePostgreSql(sqlOptions =>
            {
                sqlOptions.ConnectionString = configuration.GetConnectionString("default") ?? throw new ArgumentNullException("connectionString", "Enter connection string in app settings");
                sqlOptions.Schema = "events";
            });
        });

        services.TryAddScoped<ISessionManagement, SessionManagement>();

        services.TryAddTransient<DataSeeder>();
        services.TryAddScoped<RemoveExpiredCredentialsJob>();
        services.TryAddScoped<RemoveExpiredSessionsJob>();
        services.TryAddScoped<RemoveExpiredActivitiesJob>();
        services.AddQuartz(q =>
        {
            q.SchedulerId = "Kundera";
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 2; });
            q.ScheduleJob<SeedDataJob>(trigger => trigger
                .WithIdentity("SeedDataJob")
                .StartNow()
                .WithPriority(2)
                .WithDescription("SeedDataJob")
            );
            q.ScheduleJob<RemoveExpiredCredentialsJob>(trigger => trigger
                .WithIdentity("RemoveExpiredCredentials")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(5)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
                .WithDescription("RemoveExpiredCredentials")
            );
            q.ScheduleJob<RemoveExpiredSessionsJob>(trigger => trigger
                .WithIdentity("RemoveExpiredSessionsJob")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(20)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
                .WithDescription("RemoveExpiredSessionsJob")
            );

            q.ScheduleJob<RemoveExpiredActivitiesJob>(trigger => trigger
                .WithIdentity("RemoveExpiredActivitiesJob")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMinutes(1)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Hour))
                .WithDescription("RemoveExpiredActivitiesJob")
            );
        });
        services.AddQuartzServer(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
        });
    }
}