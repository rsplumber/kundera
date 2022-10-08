﻿using Domain.Roles;
using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record RevokeUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class RevokeUserRoleCommandHandler : CommandHandler<RevokeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public RevokeUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(RevokeUserRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, roleIds) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in roleIds)
        {
            user.RevokeRole(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}