using Core.Domains.Groups.Types;

namespace Core.Domains.Groups;

public interface IGroupRepository
{
    Task AddAsync(Group entity, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(GroupId id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Group entity, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task DeleteAsync(GroupId id, CancellationToken cancellationToken = default);
}