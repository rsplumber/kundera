using System.Reflection;
using Kite.CQRS.InMemory;
using Managements.Data.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managements.Builder;

public static class ServiceCollectionExtension
{
    public static void AddManagements(this IServiceCollection services, IConfiguration configuration)
    {
        var dataAssembly = services.AddManagementsData(configuration);
        services.AddCqrsInMemory(Assembly.Load("Managements.Application"), dataAssembly);
    }
}