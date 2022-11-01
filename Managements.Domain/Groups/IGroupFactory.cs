using Managements.Domain.Groups.Exception;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Domain.Groups;

public interface IGroupFactory
{
    Task<Group> CreateAsync(Name name, RoleId role, GroupId? parent = null);

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

    public async Task<Group> CreateAsync(Name name, RoleId roleId, GroupId? parent = null)
    {
        var existsGroup = await _groupRepository.FindAsync(name);
        if (existsGroup is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var role = await _roleRepository.FindAsync(roleId);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        Group? selectedParent;
        if (parent is null)
        {
            selectedParent = await _groupRepository.FindAsync(EntityBaseValues.AdministratorGroup);
        }
        else
        {
            selectedParent = await _groupRepository.FindAsync(parent);
        }

        if (selectedParent is null)
        {
            throw new GroupNotFoundException();
        }


        var group = new Group(name, role.Id, selectedParent.Id);
        await _groupRepository.AddAsync(group);

        selectedParent.AddChild(group.Id);
        await _groupRepository.UpdateAsync(selectedParent);

        return group;
    }

    public async Task<Group> CreateAdministratorAsync()
    {
        var existsGroup = await _groupRepository.FindAsync(EntityBaseValues.AdministratorGroup);
        if (existsGroup is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var role = await _roleRepository.FindAsync(EntityBaseValues.SuperAdminRole);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var group = new Group(EntityBaseValues.AdministratorGroup, role.Id);
        await _groupRepository.AddAsync(group);

        return group;
    }
}