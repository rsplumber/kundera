using Application.Auth.Sessions;
using Application.BackgroundJobs;
using Core.Domains.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Sessions;
using Core.Domains.Users;
using Core.Hashing;
using Core.Services;
using Data.Seeder;
using Hashing.HMAC;
using Managements.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediator(c => c.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddCap(x =>
        {
            x.UseInMemoryStorage();
            x.UseInMemoryMessageQueue();
        });
        services.TryAddSingleton<IHashService>(_ => new HmacHashingService(HashingType.HMACSHA384, 6));
        services.AddScoped<ISessionManagement, SessionManagement>();
        services.Configure<SessionOptions>(configuration.GetSection("Sessions"));

        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IScopeFactory, ScopeFactory>();
        services.AddScoped<IRoleFactory, RoleFactory>();
        services.AddScoped<IPermissionFactory, PermissionFactory>();
        services.AddScoped<IGroupFactory, GroupFactory>();
        services.AddScoped<ISessionFactory, SessionFactory>();
        services.AddScoped<ICredentialFactory, CredentialFactory>();
        services.AddTransient<RemoveExpiredCredentialsJob>();
        services.AddTransient<RemoveExpiredSessionsJob>();

        services.AddData(configuration);
        services.AddDataSeeder();

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