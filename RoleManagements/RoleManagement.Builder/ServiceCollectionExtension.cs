using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleManagement.Data;
using Tes.CQRS.InMemory;
using Tes.Serializer.Microsoft;

namespace RoleManagement.Builder;

public static class ServiceCollectionExtension
{
    public static void AddRoleManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        var assemblies = new[]
        {
            Assembly.Load("RoleManagement.Application"),
            Assembly.Load("RoleManagement.Data")
        };
        services.AddCqrsInMemory(configuration, assemblies);
        services.AddDataLayer(configuration);
    }
}