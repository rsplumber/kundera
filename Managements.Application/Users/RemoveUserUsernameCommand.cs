using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record RemoveUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class RemoveUserUsernameCommandHandler : ICommandHandler<RemoveUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(RemoveUserUsernameCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, username) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveUsername(username);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}