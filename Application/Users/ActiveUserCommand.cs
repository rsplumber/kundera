using Domain.Users;
using Domain.Users.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Users;

public sealed record ActiveUserCommand(UserId User) : Command;

internal sealed class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask HandleAsync(ActiveUserCommand message, CancellationToken cancellationToken = default)
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