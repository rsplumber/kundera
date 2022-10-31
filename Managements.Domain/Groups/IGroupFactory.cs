using Managements.Domain.Groups.Exception;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Domain.Groups;

public interface IGroupFactory
{
    Task<Group> CreateAsync(Name name, RoleId role);

    Task<Group> CreateAsync(Name name, RoleId role, GroupId parent);

    Task<Group> CreateAdministratorAsync();
}

internal sealed class GroupFactory : IGroupFactory
{
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;


    public GroupFactory(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Group> CreateAsync(Name name, RoleId roleId)
    {
        var group = await _groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var administratorGroup = await _groupRepository.FindAsync(EntityBaseValues.AdministratorGroup);
        if (administratorGroup is null)
        {
            throw new GroupNotFoundException();
        }

        var role = await _roleRepository.FindAsync(roleId);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new Group(name, role.Id, administratorGroup.Id);
    }

    public async Task<Group> CreateAsync(Name name, RoleId roleId, GroupId parent)
    {
        var group = await _groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var role = await _roleRepository.FindAsync(roleId);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new Group(name, role.Id, parent);
    }

    public async Task<Group> CreateAdministratorAsync()
    {
        var group = await _groupRepository.FindAsync(EntityBaseValues.AdministratorGroup);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var role = await _roleRepository.FindAsync(EntityBaseValues.SuperAdminRole);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new Group(EntityBaseValues.AdministratorGroup, role.Id);
    }
}