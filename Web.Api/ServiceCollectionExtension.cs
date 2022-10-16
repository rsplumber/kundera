using System.IO.Compression;
using Kite.Web.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Api;

public static class ServiceCollectionExtension
{
    public static void AddKunderaWeb(this IServiceCollection services, IConfiguration? configuration = default)
    {
        services.AddControllers();
        services.AddRequestValidators();
        services.AddCors();
        services.AddHealthChecks();
        services.AddCustomResponseCompression();
        services.AddSingleton<ExceptionMiddleware>();
    }

    private static void AddCustomResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"image/svg+xml"});
        });

        services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
    }
}