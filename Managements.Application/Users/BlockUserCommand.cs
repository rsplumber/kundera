using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;

internal sealed class BlockUserCommandHandler : ICommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(BlockUserCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, reason) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Block(reason);

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}