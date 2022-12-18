using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Core.Domains.Users.Types;
using Mediator;

namespace Application.Users;

public sealed record RemoveUserUsernameCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;
}

internal sealed class RemoveUserUsernameCommandHandler : ICommandHandler<RemoveUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(RemoveUserUsernameCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.UserId), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveUsername(command.Username);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}