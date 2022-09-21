using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Users.Application.Users;

public sealed record RemoveUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class RemoveUserUsernameCommandHandler : CommandHandler<RemoveUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(RemoveUserUsernameCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveUsername(message.Username);
    }
}