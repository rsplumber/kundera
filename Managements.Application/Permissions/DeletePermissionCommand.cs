using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Permissions;
using Managements.Domain.Permissions.Exceptions;

namespace Managements.Application.Permissions;

public sealed record DeletePermissionCommand(PermissionId PermissionId) : Command;

internal sealed class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask HandleAsync(DeletePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.FindAsync(message.PermissionId, cancellationToken);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        await _permissionRepository.DeleteAsync(message.PermissionId, cancellationToken);
    }
}