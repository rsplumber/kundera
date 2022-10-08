using Domain.Services.Types;
using Tes.Domain.Contracts;

namespace Domain.Services;

public interface IServiceRepository : IRepository<ServiceId, Service>, IUpdateService<Service>
{
    ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default);
}