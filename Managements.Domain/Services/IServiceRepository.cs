using Kite.Domain.Contracts;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public interface IServiceRepository : IRepository<Service, ServiceId>, IUpdateService<Service>, IDeleteService<ServiceId>
{
    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(ServiceSecret secret, CancellationToken cancellationToken = default);
}