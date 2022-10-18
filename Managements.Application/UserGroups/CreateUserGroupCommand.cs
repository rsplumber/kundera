﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.UserGroups;

namespace Managements.Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name, RoleId Role) : Command;

internal sealed class CreateUserGroupCommandHandler : ICommandHandler<CreateUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateUserGroupCommandHandler(IUserGroupRepository userGroupRepository, IRoleRepository roleRepository)
    {
        _userGroupRepository = userGroupRepository;
        _roleRepository = roleRepository;
    }

    public async Task HandleAsync(CreateUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var userGroup = await UserGroup.FromAsync(name, role.Id, _userGroupRepository);
        await _userGroupRepository.AddAsync(userGroup, cancellationToken);
    }
}