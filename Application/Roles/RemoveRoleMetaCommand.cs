using Domain.Roles;
using Domain.Roles.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Roles;

public sealed record RemoveRoleMetaCommand(RoleId Role, params string[] MetaKeys) : Command;

internal sealed class RemoveRoleMetaCommandHandler : CommandHandler<RemoveRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(RemoveRoleMetaCommand message, CancellationToken cancellationToken = default)
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