using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagement.Data.Services;

internal class ServiceRepository : IServiceRepository
{
    public async Task CreateAsync(Service entity, CancellationToken cancellationToken = default)
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
}