using Core.Domains.Roles.Exceptions;

namespace Core.Domains.Roles;

public interface IRoleFactory
{
    Task<Role> CreateAsync(string name, IDictionary<string, string>? meta = null);
}

public sealed class RoleFactory : IRoleFactory
{
    private readonly IRoleRepository _roleRepository;


    public RoleFactory(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> CreateAsync(string name, IDictionary<string, string>? meta = null)
    {
        var currentRole = await _roleRepository.FindByNameAsync(name);
        if (currentRole is not null)
        {
            throw new RoleAlreadyExistsException(name);
        }

        var role = new Role(name, meta);
        await _roleRepository.AddAsync(role);

        return role;
    }
}