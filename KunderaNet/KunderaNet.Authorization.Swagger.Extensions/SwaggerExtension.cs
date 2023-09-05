using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KunderaNet.Authorization.Swagger.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddKunderaHeaders(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("KunderaToken",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Input your token to access this API",
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "KunderaToken",
                        },
                        Name = "KunderaToken",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            });
        }
    }
}