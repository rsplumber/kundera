namespace Kundera.Management.Roles;

public interface IRoleService
{
    ValueTask CreateAsync(string name, IDictionary<string, string>? meta = null, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(string id, CancellationToken cancellationToken = default);

    ValueTask AddPermissionsAsync(string id, string[] permissionIds, CancellationToken cancellationToken = default);

    ValueTask RemovePermissionsAsync(string id, string[] permissionIds, CancellationToken cancellationToken = default);

    ValueTask<RoleResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<RolesResponse>> GetAsync(CancellationToken cancellationToken = default);
}