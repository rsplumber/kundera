using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record AddUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class AddUserUsernameCommandHandler : ICommandHandler<AddUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public AddUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(AddUserUsernameCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.AddUsername(message.Username);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}