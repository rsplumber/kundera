using Core.Domains.Services.Types;

namespace Core.Domains.Services;

public interface IServiceRepository
{
    Task AddAsync(Service entity, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(ServiceSecret secret, CancellationToken cancellationToken = default);

    Task UpdateAsync(Service entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(ServiceId id, CancellationToken cancellationToken = default);
}