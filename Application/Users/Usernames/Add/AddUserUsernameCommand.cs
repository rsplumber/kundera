using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Usernames.Add;

public sealed record AddUserUsernameCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;
}

internal sealed class AddUserUsernameCommandHandler : ICommandHandler<AddUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public AddUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(AddUserUsernameCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.AddUsername(command.Username);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}