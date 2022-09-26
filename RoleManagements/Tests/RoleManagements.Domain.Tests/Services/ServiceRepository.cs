using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Tests.Services;

public class ServiceRepository : IServiceRepository
{
    private readonly List<Service> _services;

    public ServiceRepository()
    {
        _services = new List<Service>();
    }


    public async Task AddAsync(Service entity, CancellationToken cancellationToken = new CancellationToken())
    {
        _services.Add(entity);
    }

    public async Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = new())
    {
        return _services.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        return _services.Exists(service => service.Id == id);
    }

    public Task UpdateAsync(Service entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}