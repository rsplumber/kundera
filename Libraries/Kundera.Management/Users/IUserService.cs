namespace Kundera.Management.Users;

public interface IUserService
{
    ValueTask CreateAsync(string username, Guid userGroupId, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    ValueTask ChangeStatusAsync(string id, UserStatus status = UserStatus.Active, CancellationToken cancellationToken = default);

    ValueTask JoinGroupAsync(Guid id, Guid userGroupId, CancellationToken cancellationToken = default);

    ValueTask RemoveGroupAsync(Guid id, Guid userGroupId, CancellationToken cancellationToken = default);

    ValueTask AddUsernameAsync(Guid id, string username, CancellationToken cancellationToken = default);

    ValueTask RemoveUsernameAsync(Guid id, string username, CancellationToken cancellationToken = default);

    ValueTask ExistUsernameAsync(string username, CancellationToken cancellationToken = default);

    ValueTask AssignRolesAsync(Guid id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask RevokeRolesAsync(Guid id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask<UserResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<UsersResponse>> GetAsync(CancellationToken cancellationToken = default);
}