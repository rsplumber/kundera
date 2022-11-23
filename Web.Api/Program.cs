using Builder;
using FastEndpoints;
using FastEndpoints.Swagger;
using KunderaNet.Authorization.Microsoft.DependencyInjection;
using NSwag;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://+:5179");

builder.Services.AddKundera(builder.Configuration);
builder.Services.AddKunderaAuthorization(builder.Configuration);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "Kundera - WebApi";
    settings.DocumentName = "v1";
    settings.Version = "v1";
    settings.AddAuth("Kundera", new()
    {
        Name = "Kundera",
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.ApiKey,
    });
}, addJWTBearerAuth: false, maxEndpointVersion: 1);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
});

// if (app.Environment.IsDevelopment())
// {
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());
// }

app.UseKundera();

await app.RunAsync();

await app.WaitForShutdownAsync();