using Domain.Permissions;
using Domain.Permissions.Exceptions;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Permissions;

public sealed record AddPermissionMetaCommand(PermissionId Permission, IDictionary<string, string> Meta) : Command;

internal sealed class AddPermissionMetaICommandHandler : ICommandHandler<AddPermissionMetaCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public AddPermissionMetaICommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask HandleAsync(AddPermissionMetaCommand message, CancellationToken cancellationToken = default)
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