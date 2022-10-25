using Managements.Domain.Permissions.Exceptions;

namespace Managements.Domain.Permissions;

public interface IPermissionFactory
{
    Task<Permission> CreateAsync(Name name);
}

internal sealed class PermissionFactory : IPermissionFactory
{
    private readonly IPermissionRepository _permissionRepository;


    public PermissionFactory(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Permission> CreateAsync(Name name)
    {
        var exists = await _permissionRepository.ExistsAsync(name);
        if (exists)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        return new Permission(name);
    }
}