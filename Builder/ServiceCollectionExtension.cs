using Auth.Application.BackgroundJobs;
using Auth.Application.Services;
using Auth.Data;
using Core.Domains.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Sessions;
using Core.Domains.Users;
using Core.Services;
using Data.Seeder;
using Hashing.HMAC;
using Kite.Hashing.HMAC.Extensions.Microsoft.DependencyInjection;
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
        services.TryAddSingleton<IHashService>(_ => new HMACHashingService(HashingType.HMACSHA384, 6));
        services.AddMediator();
        services.AddCap(x =>
        {
            var connectionString = configuration.GetConnectionString("EventSourcing");
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Enter EventSourcing connection string");
            }

            x.UseSqlServer(connectionString);
            x.UseInMemoryMessageQueue();
        });

        services.AddScoped<ISessionFactory, SessionFactory>();
        services.AddScoped<ICredentialFactory, CredentialFactory>();

        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ISessionManagement, SessionManagement>();

        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ICredentialService, CredentialService>();
        services.Configure<SessionOptions>(configuration.GetSection("Sessions"));

        services.AddAuthData(configuration);

        services.AddData(configuration);
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IScopeFactory, ScopeFactory>();
        services.AddScoped<IRoleFactory, RoleFactory>();
        services.AddScoped<IPermissionFactory, PermissionFactory>();
        services.AddScoped<IGroupFactory, GroupFactory>();

        services.AddDataSeeder();

        services.AddBackgroundServices();
    }

    private static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddTransient<RemoveExpiredCredentialsJob>();
        services.AddTransient<RemoveExpiredSessionsJob>();

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