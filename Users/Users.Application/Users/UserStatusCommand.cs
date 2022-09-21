using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ActiveUserCommand(UserId User, Text? Reason) : Command;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;

internal sealed class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand, ActiveUserCommandHandler>
{
    public Task<ActiveUserCommandHandler> HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class SuspendUserCommandHandler : ICommandHandler<SuspendUserCommand, SuspendUserCommandHandler>
{
    public Task<SuspendUserCommandHandler> HandleAsync(SuspendUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class BlockUserCommandHandler : ICommandHandler<BlockUserCommand, BlockUserCommandHandler>
{
    public Task<BlockUserCommandHandler> HandleAsync(BlockUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}