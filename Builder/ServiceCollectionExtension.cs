using System.Reflection;
using Authentication.Infrastructure;
using Data.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.CQRS.InMemory;
using Tes.Serializer.Microsoft;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddCqrsInMemory(configuration, new[]
        {
            Assembly.Load("Application"),
            Assembly.Load("Data.Redis")
        });
        services.AddDataRedis(configuration);
        services.AddAuth(configuration);
    }
}