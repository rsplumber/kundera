using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Mediator;

namespace Application.Users;

public sealed record AssignUserRoleCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class AssignUserRoleCommandHandler : ICommandHandler<AssignUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public AssignUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(AssignUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in command.RolesIds)
        {
            user.AssignRole(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}