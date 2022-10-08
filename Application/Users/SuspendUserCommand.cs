using Domain;
using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

internal sealed class SuspendUserCommandHandler : CommandHandler<SuspendUserCommand>
{
    private readonly IUserRepository _userRepository;

    public SuspendUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(SuspendUserCommand message, CancellationToken cancellationToken = default)
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