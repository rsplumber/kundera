using Domain.Permissions;
using Domain.Permissions.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Permissions;

public sealed record DeletePermissionCommand(PermissionId PermissionId) : Command;

internal sealed class DeletePermissionCommandHandler : CommandHandler<DeletePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public override async Task HandleAsync(DeletePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.FindAsync(message.PermissionId, cancellationToken);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        await _permissionRepository.DeleteAsync(message.PermissionId, cancellationToken);
    }
}