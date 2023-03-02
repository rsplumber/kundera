﻿using Application.Auth;
using Application.Auth.Sessions;
using Application.Seeders;
using Core.Domains.Auth.Sessions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Application;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediator(c => c.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddCap(x =>
        {
            x.UseInMemoryStorage();
            x.UseInMemoryMessageQueue();
            x.UseDashboard();
        });
        
        services.AddScoped<ISessionManagement, SessionManagement>();
        services.Configure<SessionDefaultOptions>(configuration.GetSection("Sessions"));

        services.TryAddTransient<DataSeeder>();
        services.AddScoped<RemoveExpiredCredentialsJob>();
        services.AddScoped<RemoveExpiredSessionsJob>();
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
            // q.ScheduleJob<RemoveExpiredSessionsJob>(trigger => trigger
            //     .WithIdentity("RemoveExpiredSessionsJob")
            //     .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
            //     .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
            //     .WithDescription("RemoveExpiredSessionsJob")
            // );
        });
        services.AddQuartzServer(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
        });
    }
}