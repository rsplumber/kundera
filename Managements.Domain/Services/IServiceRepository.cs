using Kite.Domain.Contracts;

namespace Managements.Domain.Services;

public interface IServiceRepository : IRepository<Service, ServiceId>, IUpdateService<Service>, IDeleteService<ServiceId>
{
    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);
}