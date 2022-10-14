using Domain.Users;
using Domain.Users.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Users;

public sealed record DeleteUserCommand(UserId UserId) : Command;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async ValueTask HandleAsync(DeleteUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(message.UserId, cancellationToken);
    }
}