using Builder;
using FastEndpoints;
using FastEndpoints.Swagger;
using KunderaNet.FastEndpoints.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://+:5179");
builder.Services.AddKundera(builder.Configuration);

builder.Services.AddAuthentication(KunderaDefaults.Scheme)
    .AddKundera(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "Kundera - WebApi";
    settings.DocumentName = "v1";
    settings.Version = "v1";
    settings.AddKunderaAuth();
}, addJWTBearerAuth: false, maxEndpointVersion: 1);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
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