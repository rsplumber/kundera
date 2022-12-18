using Core.Domains.Permissions;
using Core.Domains.Permissions.Exceptions;
using Core.Domains.Permissions.Types;
using Mediator;

namespace Application.Permissions;

public sealed record DeletePermissionCommand : ICommand
{
    public Guid PermissionId { get; init; } = default;
}

internal sealed class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(DeletePermissionCommand command, CancellationToken cancellationToken)
    {
        var permissionId = PermissionId.From(command.PermissionId);
        var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        await _permissionRepository.DeleteAsync(permissionId, cancellationToken);

        return Unit.Value;
    }
}