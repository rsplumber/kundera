namespace Core.Groups;

public interface IGroupRepository
{
    Task AddAsync(Group entity, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Group entity, CancellationToken cancellationToken = default);

    Task<Group?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}