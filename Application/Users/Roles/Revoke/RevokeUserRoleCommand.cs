using Core.Roles;
using Core.Roles.Exceptions;
using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Roles.Revoke;

public sealed record RevokeUserRoleCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class RevokeUserRoleCommandHandler : ICommandHandler<RevokeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public RevokeUserRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
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
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }
            user.Revoke(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}