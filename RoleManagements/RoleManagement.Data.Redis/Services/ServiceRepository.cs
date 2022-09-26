using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagement.Data.Redis.Services;

internal class ServiceRepository : IServiceRepository
{
    public async Task AddAsync(Service entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Service entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}