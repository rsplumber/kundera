using Kite.Domain.Contracts;

namespace Managements.Domain.Groups;

public interface IGroupRepository : IRepository<Group, GroupId>, IUpdateService<Group>, IDeleteService<GroupId>
{
    Task<IEnumerable<Group>> FindAsync(IEnumerable<GroupId> groupIds, CancellationToken cancellationToken = default);

    Task<IEnumerable<Group>> FindChildrenAsync(GroupId id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Group>> FindParentsAsync(GroupId id, CancellationToken cancellationToken = default);

    Task<Group?> FindAsync(Name name, CancellationToken cancellationToken = default);
}