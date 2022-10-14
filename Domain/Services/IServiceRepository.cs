using Kite.Domain.Contracts;

namespace Domain.Services;

public interface IServiceRepository : IRepository<Service, ServiceId>, IUpdateService<Service>, IDeleteService<ServiceId>
{
    ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default);
}