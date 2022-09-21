﻿using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record RemoveUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class RemoveUserUsernameCommandHandler : ICommandHandler<RemoveUserUsernameCommand, RemoveUserUsernameCommandHandler>
{
    public Task<RemoveUserUsernameCommandHandler> HandleAsync(RemoveUserUsernameCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}
