using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
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
    private readonly IRoleRepository _roleRepository;

    public AssignUserRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(AssignUserRoleCommand command, CancellationToken cancellationToken)
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
            user.Assign(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}