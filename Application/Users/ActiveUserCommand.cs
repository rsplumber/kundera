﻿using Domain;
using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record ActiveUserCommand(UserId User, Text? Reason) : Command;

internal sealed class ActiveUserCommandHandler : CommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, reason) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Activate(reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}