using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public interface IServiceRepository
{
    Task AddAsync(Service entity, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(ServiceSecret secret, CancellationToken cancellationToken = default);

    Task UpdateAsync(Service entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(ServiceId id, CancellationToken cancellationToken = default);
}