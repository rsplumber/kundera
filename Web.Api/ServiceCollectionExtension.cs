using Kite.Web.Requests;
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
    }
}