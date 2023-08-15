using System.Text.Json;
using Application;
using Application.Auth;
using Application.Seeders;
using Core;
using Data;
using Data.Abstractions;
using Data.Caching.Abstractions;
using Elastic.Apm.NetCoreAll;
using FastEndpoints;
using FastEndpoints.Swagger;
using KunderaNet.FastEndpoints.Authorization;
using KunderaNet.Services.Authorization.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel((_, options) =>
{
    options.ListenAnyIP(1002, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
    // options.ListenAnyIP(5179, listenOptions =>
    // {
    //     listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    //     listenOptions.UseHttps();
    // });
});
var configuration = builder.Configuration;
builder.Services.AddCors();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(KunderaDefaults.Scheme)
    .AddKundera(builder.Configuration, k => k.UseHttpService(builder.Configuration));
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(settings =>
{
    settings.DocumentSettings = generatorSettings =>
    {
        generatorSettings.Title = "Kundera - WebApi";
        generatorSettings.DocumentName = "v1";
        generatorSettings.Version = "v1";
        generatorSettings.AddKunderaAuth();
    };
    settings.EnableJWTBearerAuth = false;
    settings.MaxEndpointVersion = 1;
});


builder.Services.TryAddSingleton<ExceptionHandlerMiddleware>();
builder.Services.AddCore(builder.Configuration);
builder.Services.AddCap(options =>
{
    options.FailedRetryCount = 2;
    options.FailedRetryInterval = 5;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
    options.SucceedMessageExpiredAfter = 60 * 5;
    options.FailedMessageExpiredAfter = 60 * 5;
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


builder.Services.TryAddTransient<DataSeeder>();
builder.Services.TryAddScoped<RemoveExpiredCredentialsJob>();
builder.Services.TryAddScoped<RemoveExpiredSessionsJob>();
builder.Services.TryAddScoped<RemoveExpiredActivitiesJob>();
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "Kundera";
    q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 1; });
    q.ScheduleJob<SeedDataJob>(trigger => trigger
        .WithIdentity("SeedDataJob")
        .StartNow()
        .WithPriority(2)
        .WithDescription("SeedDataJob")
    );
    q.ScheduleJob<RemoveExpiredCredentialsJob>(trigger => trigger
        .WithIdentity("RemoveExpiredCredentials")
        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMinutes(2)))
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(2, IntervalUnit.Hour))
        .WithDescription("RemoveExpiredCredentials")
    );
    q.ScheduleJob<RemoveExpiredSessionsJob>(trigger => trigger
        .WithIdentity("RemoveExpiredSessionsJob")
        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(5)))
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(5, IntervalUnit.Hour))
        .WithDescription("RemoveExpiredSessionsJob")
    );

    q.ScheduleJob<RemoveExpiredActivitiesJob>(trigger => trigger
        .WithIdentity("RemoveExpiredActivitiesJob")
        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddMinutes(10)))
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(24, IntervalUnit.Hour))
        .WithDescription("RemoveExpiredActivitiesJob")
    );
});
builder.Services.AddQuartzServer(options =>
{
    options.AwaitApplicationStarted = true;
    options.WaitForJobsToComplete = true;
});

builder.Services.AddData(options =>
{
    options.UseEntityFramework(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default") ??
                                                                          throw new ArgumentNullException("connectionString", "Enter connection string in app settings")));

    options.AddCaching();
});
builder.Services.AddMediator(c => c.ServiceLifetime = ServiceLifetime.Scoped);

var app = builder.Build();
app.Services.UseData(options => { options.UseEntityFramework(); });

app.UseHealthChecks("/health");
app.UseCors(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseAllElasticApm(builder.Configuration);

app.UseFastEndpoints(config =>
{
    config.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
});

// if (app.Environment.IsDevelopment())
// {
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());
// }


await app.RunAsync();