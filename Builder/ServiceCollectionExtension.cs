using System.Reflection;
using Authentication.Infrastructure;
using Data.Redis;
using Kite.CQRS.InMemory;
using Kite.Serializer.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer();
        services.AddCqrsInMemory(Assembly.Load("Application"), Assembly.Load("Data.Redis"));
        services.AddAuth(configuration);
        services.AddDataRedis(configuration);
    }
}