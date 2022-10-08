using Domain.Permissions;
using Domain.Permissions.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Permissions;

public sealed record RemovePermissionMetaCommand(PermissionId Permission, params string[] MetaKeys) : Command;

internal sealed class RemovePermissionMetaCommandHandler : CommandHandler<RemovePermissionMetaCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public RemovePermissionMetaCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public override async Task HandleAsync(RemovePermissionMetaCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        var (permissionId, metaKeys) = message;
        var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        foreach (var meta in metaKeys)
        {
            permission.RemoveMeta(meta);
        }

        await _permissionRepository.UpdateAsync(permission, cancellationToken);
    }
}