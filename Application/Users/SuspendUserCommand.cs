using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Mediator;

namespace Application.Users;

public sealed record SuspendUserCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string? Reason { get; init; }
}

internal sealed class SuspendUserCommandHandler : ICommandHandler<SuspendUserCommand>
{
    private readonly IUserRepository _userRepository;

    public SuspendUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(SuspendUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Suspend(command.Reason);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}