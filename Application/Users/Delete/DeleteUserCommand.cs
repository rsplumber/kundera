using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Delete;

public sealed record DeleteUserCommand : ICommand
{
    public Guid UserId { get; init; } = default!;
}

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async ValueTask<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(user.Id, cancellationToken);
        return Unit.Value;
    }
}