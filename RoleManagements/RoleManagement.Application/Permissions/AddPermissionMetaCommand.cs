using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Exceptions;
using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record AddPermissionMetaCommand(PermissionId Permission, IDictionary<string, string> Meta) : Command;

internal sealed class AddPermissionMetaCommandHandler : CommandHandler<AddPermissionMetaCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public AddPermissionMetaCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public override async Task HandleAsync(AddPermissionMetaCommand message, CancellationToken cancellationToken = default)
    {
        var (permissionId, dictionary) = message;
        var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        foreach (var (key, value) in dictionary)
        {
            permission.AddMeta(key, value);
        }

        await _permissionRepository.UpdateAsync(permission, cancellationToken);
    }
}