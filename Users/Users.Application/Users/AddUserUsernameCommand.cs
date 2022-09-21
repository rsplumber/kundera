using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Users.Application.Users;

public sealed record AddUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class AddUserUsernameCommandHandler : CommandHandler<AddUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public AddUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(AddUserUsernameCommand message, CancellationToken cancellationToken = default)
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