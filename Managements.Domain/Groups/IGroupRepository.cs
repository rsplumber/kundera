using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups;

public interface IGroupRepository
{
    Task AddAsync(Group entity, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(GroupId id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Group entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<Group>> FindAsync(IEnumerable<GroupId> groupIds, CancellationToken cancellationToken = default);

    Task<IEnumerable<Group>> FindChildrenAsync(GroupId id, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(Name name, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(GroupId id, CancellationToken cancellationToken = default);
}