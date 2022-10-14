using Domain;
using Domain.Users;
using Domain.Users.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Users;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

internal sealed class SuspendUserCommandHandler : ICommandHandler<SuspendUserCommand>
{
    private readonly IUserRepository _userRepository;

    public SuspendUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask HandleAsync(SuspendUserCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, reason) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Suspend(reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}