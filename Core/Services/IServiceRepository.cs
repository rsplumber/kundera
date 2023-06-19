namespace Core.Services;

public interface IServiceRepository
{
    Task AddAsync(Service entity, CancellationToken cancellationToken = default);

    Task<Service?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Service?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Service?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default);

    Task UpdateAsync(Service entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}