﻿using Domain;
using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;

internal sealed class BlockUserCommandHandler : CommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(BlockUserCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, reason) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Block(reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}