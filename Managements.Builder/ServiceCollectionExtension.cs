using System.Reflection;
using Kite.CQRS.InMemory.Microsoft.DependencyInjection;
using Kite.Events.InMemory.Extensions.Microsoft.DependencyInjection;
using Managements.Data;
using Managements.Domain.Groups;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managements.Builder;

public static class ServiceCollectionExtension
{
    public static void AddManagements(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IScopeFactory, ScopeFactory>();
        services.AddScoped<IRoleFactory, RoleFactory>();
        services.AddScoped<IPermissionFactory, PermissionFactory>();
        services.AddScoped<IGroupFactory, GroupFactory>();
        var dataAssembly = services.AddManagementsData(configuration);
        var applicationAssembly = Assembly.Load("Managements.Application");
        services.AddCqrsInMemory(applicationAssembly, dataAssembly);
        
        services.AddEventsInMemory(dataAssembly);
    }
}