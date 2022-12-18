using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Core.Domains.Users.Types;
using Mediator;

namespace Application.Users;

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
        var userId = UserId.From(command.UserId);
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(userId, cancellationToken);
        return Unit.Value;
    }
}