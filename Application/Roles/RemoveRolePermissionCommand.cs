using Core.Domains.Permissions.Types;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Roles.Types;
using Mediator;

namespace Application.Roles;

public sealed record RemoveRolePermissionCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public Guid[] Permissions { get; init; } = default!;
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
        var role = await _roleRepository.FindAsync(RoleId.From(command.RoleId), cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permission in command.Permissions)
        {
            role.RemovePermission(PermissionId.From(permission));
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}