using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Tests.Services;

public class ServiceRepository : IServiceRepository
{
    private static readonly List<Service> Services = new();

    public async Task CreateAsync(Service entity, CancellationToken cancellationToken = new())
    {
        Services.Add(entity);
    }

    public async Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = new())
    {
        return Services.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        return Services.Exists(service => service.Id == id);
    }
}