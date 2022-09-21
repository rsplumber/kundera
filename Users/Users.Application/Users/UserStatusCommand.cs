using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Users.Application.Users;

public sealed record ActiveUserCommand(UserId User, Text? Reason) : Command;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;

internal sealed class ActiveUserCommandHandler : CommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Activate(message.Reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}

internal sealed class SuspendUserCommandHandler : CommandHandler<SuspendUserCommand>
{
    private readonly IUserRepository _userRepository;

    public SuspendUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(SuspendUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Suspend(message.Reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}

internal sealed class BlockUserCommandHandler : CommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(BlockUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Block(message.Reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}