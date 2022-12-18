using System.Collections.Immutable;
using Core.Domains.Permissions;
using Core.Domains.Permissions.Exceptions;
using Core.Domains.Permissions.Types;
using Mediator;

namespace Application.Permissions;

public sealed record ChangePermissionMetaCommand : ICommand
{
    public Guid PermissionId { get; init; } = default!;

    public IDictionary<string, string> Meta { get; init; } = ImmutableDictionary<string, string>.Empty;
}

internal sealed class ChangePermissionMetaCommandHandler : ICommandHandler<ChangePermissionMetaCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public ChangePermissionMetaCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(ChangePermissionMetaCommand command, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.FindAsync(PermissionId.From(command.PermissionId), cancellationToken);

        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        permission.Meta.Clear();
        foreach (var (key, value) in command.Meta)
        {
            permission.Meta.Add(key, value);
        }

        await _permissionRepository.UpdateAsync(permission, cancellationToken);

        return Unit.Value;
    }
}