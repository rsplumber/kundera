using Core.Permissions.Exceptions;
using Core.Services;

namespace Core.Permissions;

public interface IPermissionService
{
    ValueTask<Permission> CreateAsync(Service service, string name, IDictionary<string, string>? meta = null, CancellationToken cancellationToken = default);
}

internal sealed class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Permission> CreateAsync(Service service, string name, IDictionary<string, string>? meta = null, CancellationToken cancellationToken = default)
    {
        var permission = new Permission(service, name, meta);
        if (service.Permissions.Any(p => p.Name == permission.Name))
        {
            throw new PermissionAlreadyExistsException(permission.Name);
        }

        await _permissionRepository.AddAsync(permission, cancellationToken);
        return permission;
    }
}