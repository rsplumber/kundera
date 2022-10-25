using Managements.Domain.Roles.Exceptions;

namespace Managements.Domain.Roles;

public interface IRoleFactory
{
    Task<Role> CreateAsync(Name name);
}

internal sealed class RoleFactory : IRoleFactory
{
    private readonly IRoleRepository _roleRepository;


    public RoleFactory(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> CreateAsync(Name name)
    {
        var exists = await _roleRepository.ExistsAsync(name);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        return new Role(name);
    }
}