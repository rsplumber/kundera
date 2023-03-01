using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;

namespace Application.Roles;

public sealed record RemoveRolePermissionCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public Guid[] PermissionsIds { get; init; } = default!;
}

internal sealed class RemoveRolePermissionCommandHandler : ICommandHandler<RemoveRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRolePermissionCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(RemoveRolePermissionCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permission in command.PermissionsIds)
        {
            role.RemovePermission(permission);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}