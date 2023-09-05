using FastEndpoints.Swagger;
using NSwag;
using NSwag.Generation;
using NSwag.Generation.AspNetCore;

namespace KunderaNet.FastEndpoints.Authorization;

public static class SwaggerExtension
{
    public static OpenApiDocumentGeneratorSettings AddKunderaAuth(this AspNetCoreOpenApiDocumentGeneratorSettings settings)
    {
        return settings.AddAuth("Authorization", new()
        {
            Description = "Kundera Token",
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Type = OpenApiSecuritySchemeType.ApiKey,
        });
    }
}