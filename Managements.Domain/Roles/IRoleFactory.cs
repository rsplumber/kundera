using Managements.Domain.Roles.Exceptions;

namespace Managements.Domain.Roles;

public interface IRoleFactory
{
    Task<Role> CreateAsync(Name name, IDictionary<string, string>? meta = null);
}

internal sealed class RoleFactory : IRoleFactory
{
    private readonly IRoleRepository _roleRepository;


    public RoleFactory(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> CreateAsync(Name name, IDictionary<string, string>? meta = null)
    {
        var exists = await _roleRepository.ExistsAsync(name);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        var role = new Role(name, meta);
        await _roleRepository.AddAsync(role);

        return role;
    }
}