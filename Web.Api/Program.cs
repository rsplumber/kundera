using System.Text.Json;
using Builder;
using FastEndpoints;
using FastEndpoints.Swagger;
using KunderaNet.FastEndpoints.Authorization;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://+:5179");

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

builder.Services.AddKundera(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(config =>
{
    config.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
    config.Endpoints.Configurator = ep =>
    {
        // ep.PostProcessors(0, new ExceptionErrorBuilder());
        ep.Description(b => b.Produces<CustomErrorResponse>(400));
    };
    config.Errors.ResponseBuilder = (failures, _, _) =>
    {
        return new CustomErrorResponse
        {
            Message = "Validation failed",
            Errors = failures.Select(failure => $"{failure.PropertyName} : {failure.ErrorMessage}").ToList()
        };
    };
});

// if (app.Environment.IsDevelopment())
// {
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());
// }

app.UseKundera();

await app.RunAsync();