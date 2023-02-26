using System.Text.Json;
using Commands.Auth.Sessions;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Core.Hashing;
using FastEndpoints;
using FastEndpoints.Swagger;
using Jobs;
using KunderaNet.FastEndpoints.Authorization;
using Managements.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savorboard.CAP.InMemoryMessageQueue;
using Seeders;
using Web;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel((_, options) =>
{
    options.ListenAnyIP(5178, _ => { });
    options.ListenAnyIP(5179, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});

builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

builder.Services.AddAuthentication(KunderaDefaults.Scheme)
    .AddKundera(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddCors();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "Kundera - WebApi";
    settings.DocumentName = "v1";
    settings.Version = "v1";
    settings.AddKunderaAuth();
}, addJWTBearerAuth: false, maxEndpointVersion: 1);

AddDependencies();

var app = builder.Build();


app.UseCors(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());


app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
// app.UseAllElasticApm(builder.Configuration);

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

app.UseData();

await app.RunAsync();

void AddDependencies()
{
    builder.Services.AddMediator(c => c.ServiceLifetime = ServiceLifetime.Scoped);
    builder.Services.AddCap(x =>
    {
        x.UseInMemoryStorage();
        x.UseInMemoryMessageQueue();
        x.UseDashboard();
    });

    builder.Services.TryAddSingleton<IHashService>(_ => new HmacHashingService(HashingType.HMACSHA384, 6));
    builder.Services.AddScoped<ISessionManagement, SessionManagement>();
    builder.Services.Configure<SessionDefaultOptions>(builder.Configuration.GetSection("Sessions"));

    builder.Services.AddScoped<IUserFactory, UserFactory>();
    builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
    builder.Services.AddScoped<IScopeFactory, ScopeFactory>();
    builder.Services.AddScoped<IRoleFactory, RoleFactory>();
    builder.Services.AddScoped<IPermissionFactory, PermissionFactory>();
    builder.Services.AddScoped<IGroupFactory, GroupFactory>();
    builder.Services.AddScoped<ISessionFactory, SessionFactory>();
    builder.Services.AddScoped<ICredentialFactory, CredentialFactory>();

    builder.Services.AddData(builder.Configuration);
    builder.Services.AddJobs(builder.Configuration);
    builder.Services.AddDataSeeders(builder.Configuration);
}