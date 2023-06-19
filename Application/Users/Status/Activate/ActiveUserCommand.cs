using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Status.Activate;

public sealed record ActiveUserCommand : ICommand
{
    public Guid UserId { get; init; } = default!;
}

internal sealed class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async ValueTask<Unit> Handle(ActiveUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Activate();

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}