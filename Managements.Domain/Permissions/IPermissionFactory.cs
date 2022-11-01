using Managements.Domain.Permissions.Exceptions;

namespace Managements.Domain.Permissions;

public interface IPermissionFactory
{
    Task<Permission> CreateAsync(Name name, IDictionary<string, string>? meta = null);
}

internal sealed class PermissionFactory : IPermissionFactory
{
    private readonly IPermissionRepository _permissionRepository;


    public PermissionFactory(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Permission> CreateAsync(Name name, IDictionary<string, string>? meta = null)
    {
        var exists = await _permissionRepository.ExistsAsync(name);
        if (exists)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        var permission = new Permission(name, meta);
        await _permissionRepository.AddAsync(permission);
        return permission;
    }
}