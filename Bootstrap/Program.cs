using Builder;
using Microsoft.OpenApi.Models;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

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

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Token",
                },
                Scheme = "Token",
                Name = "Token",
                In = ParameterLocation.Header,
            },
            new List<string>()
        },
    });
});

builder.Services.AddKundera(configuration);
builder.Services.AddKunderaWeb();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseKundera();
app.UseKunderaWeb();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

await app.WaitForShutdownAsync();