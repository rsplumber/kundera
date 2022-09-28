using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.CQRS.InMemory;
using Tes.Serializer.Microsoft;
using Users.Data.Redis;

namespace Users.Builder;

public static class ServiceCollectionExtension
{
    public static void AddUsers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddCqrsInMemory(configuration, new[]
        {
            Assembly.Load("Users.Application"),
            Assembly.Load("Users.Data.Redis")
        });
        services.AddDataLayer(configuration);
    }
}