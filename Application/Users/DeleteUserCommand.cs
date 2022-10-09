using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record DeleteUserCommand(UserId UserId) : Command;

internal sealed class DeleteUserCommandHandler : CommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public override async Task HandleAsync(DeleteUserCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(message.UserId, cancellationToken);
    }
}