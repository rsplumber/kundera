﻿using Core.Groups.Exception;
using Core.Roles;
using Core.Roles.Exceptions;

namespace Core.Groups;

public interface IGroupFactory
{
    Task<Group> CreateAsync(string name, Guid roleId, Guid? parentId = null);

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

    public async Task<Group> CreateAsync(string name, Guid roleId, Guid? parent = null)
    {
        var existsGroup = await _groupRepository.FindByNameAsync(name);
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
            selectedParent = await _groupRepository.FindByNameAsync(EntityBaseValues.AdministratorGroup);
        }
        else
        {
            selectedParent = await _groupRepository.FindAsync(parent.Value);
        }

        if (selectedParent is null)
        {
            throw new GroupNotFoundException();
        }


        var group = new Group(name, role, selectedParent);
        await _groupRepository.AddAsync(group);

        selectedParent.Add(group);
        await _groupRepository.UpdateAsync(selectedParent);

        return group;
    }

    public async Task<Group> CreateAdministratorAsync()
    {
        var existsGroup = await _groupRepository.FindByNameAsync(EntityBaseValues.AdministratorGroup);
        if (existsGroup is not null)
        {
            throw new GroupNameDuplicateException();
        }

        var role = await _roleRepository.FindByNameAsync(EntityBaseValues.SuperAdminRole);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var group = new Group(EntityBaseValues.AdministratorGroup, role);
        await _groupRepository.AddAsync(group);

        return group;
    }
}