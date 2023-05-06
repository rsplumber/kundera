using System.Text.Json;
using Application;
using Core;
using Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using KunderaNet.FastEndpoints.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel((_, options) =>
{
    options.ListenAnyIP(5178, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
    // options.ListenAnyIP(5179, listenOptions =>
    // {
    //     listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    //     listenOptions.UseHttps();
    // });
});

builder.Services.AddCors();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(KunderaDefaults.Scheme)
    .AddKundera(builder.Configuration);
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
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddData(builder.Configuration);
builder.Services.AddMediator(c => c.ServiceLifetime = ServiceLifetime.Scoped);


var app = builder.Build();
app.UseData();
app.UseHealthChecks("/health");
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


await app.RunAsync();