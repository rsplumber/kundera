using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Mediator;

namespace Application.Users;

public sealed record RevokeUserRoleCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class RevokeUserRoleCommandHandler : ICommandHandler<RevokeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public RevokeUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(RevokeUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var roleId in command.RolesIds)
        {
            user.RevokeRole(roleId);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}