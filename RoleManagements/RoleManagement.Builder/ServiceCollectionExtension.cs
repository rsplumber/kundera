using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleManagement.Data.Redis;
using Tes.CQRS.InMemory;
using Tes.Serializer.Microsoft;

namespace RoleManagement.Builder;

public static class ServiceCollectionExtension
{
    public static void AddRoleManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddCqrsInMemory(configuration, new[]
        {
            Assembly.Load("RoleManagement.Application"),
            Assembly.Load("RoleManagement.Data.Redis")
        });
        services.AddDataLayer(configuration);
    }
}