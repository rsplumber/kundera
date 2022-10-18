using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record ActiveUserCommand(UserId User) : Command;

internal sealed class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Activate();

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}