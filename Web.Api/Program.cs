using Builder;
using Kite.Web.Requests;
using KunderaNet.Authorization.Microsoft.DependencyInjection;
using KunderaNet.Authorization.Swagger.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddKundera(builder.Configuration);
builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddKunderaAuthorization(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddRequestValidators();
builder.Services.AddCors();
builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"image/svg+xml"});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Kundera - WebApi",
            Description = "This Api will be responsible for overall data distribution and authorization.",
            Contact = new OpenApiContact
            {
                Name = "plumber",
                Email = "sha.a.wf@gmail.com"
            }
        });
    c.AddKunderaHeaders();
});

var app = builder.Build();

app.UseKundera();
app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();
app.UseHealthChecks("/health");
app.UseCors(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

await app.RunAsync();