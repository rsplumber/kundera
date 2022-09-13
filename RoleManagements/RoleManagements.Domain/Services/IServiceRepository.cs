using RoleManagements.Domain.Services.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Services;

public interface IServiceRepository : IRepository<ServiceId, Service>
{
    ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default);
}