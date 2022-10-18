using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Roles;

public sealed record RemoveRoleMetaCommand(RoleId Role, params string[] MetaKeys) : Command;

internal sealed class RemoveRoleMetaCommandHandler : ICommandHandler<RemoveRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task HandleAsync(RemoveRoleMetaCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, metaKeys) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var metaKey in metaKeys)
        {
            role.RemoveMeta(metaKey);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
    }
}