using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ActiveUserCommand(UserId User, Text? Reason) : Command;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;

internal sealed class ActiveUserCommandHandler : CommandHandler<ActiveUserCommand>
{
    public override Task HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class SuspendUserCommandHandler : CommandHandler<SuspendUserCommand >
{
    public override Task HandleAsync(SuspendUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class BlockUserCommandHandler : CommandHandler<BlockUserCommand>
{
    public override Task HandleAsync(BlockUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}
