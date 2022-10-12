namespace Kundera.Management.UserGroups;

public interface IUserGroupService
{
    ValueTask CreateAsync(string name, string roleId, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    ValueTask ChangeStatusAsync(string id, UserGroupStatus status = UserGroupStatus.Enable, CancellationToken cancellationToken = default);

    ValueTask SetParentAsync(Guid id, Guid parentId, CancellationToken cancellationToken = default);

    ValueTask RemoveParentAsync(Guid id, Guid parentId, CancellationToken cancellationToken = default);

    ValueTask MoveParentAsync(Guid userGroupId, Guid parentId, CancellationToken cancellationToken = default);

    ValueTask AssignRolesAsync(Guid id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask RevokeRolesAsync(Guid id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask<UserGroupResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<UserGroupsResponse>> GetAsync(CancellationToken cancellationToken = default);
}