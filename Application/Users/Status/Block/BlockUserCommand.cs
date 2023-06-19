using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Status.Block;

public sealed record BlockUserCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Reason { get; init; } = default!;
}

internal sealed class BlockUserCommandHandler : ICommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(BlockUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Block(command.Reason);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}