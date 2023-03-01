using Core.Domains.Permissions.Exceptions;

namespace Core.Domains.Permissions;

public interface IPermissionFactory
{
    Task<Permission> CreateAsync(string name, IDictionary<string, string>? meta = null);
}

internal sealed class PermissionFactory : IPermissionFactory
{
    private readonly IPermissionRepository _permissionRepository;


    public PermissionFactory(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Permission> CreateAsync(string name, IDictionary<string, string>? meta = null)
    {
        var currentPermission = await _permissionRepository.FindByNameAsync(name);
        if (currentPermission is not null)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        var permission = new Permission(name, meta);
        await _permissionRepository.AddAsync(permission);
        return permission;
    }
}