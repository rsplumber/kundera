using System.Reflection;
using Kite.CQRS.InMemory.Microsoft.DependencyInjection;
using Managements.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managements.Builder;

public static class ServiceCollectionExtension
{
    public static void AddManagements(this IServiceCollection services, IConfiguration configuration)
    {
        var dataAssembly = services.AddManagementsData(configuration);
        var applicationAssembly = Assembly.Load("Managements.Application");
        services.AddCqrsInMemory(applicationAssembly, dataAssembly);
    }
}