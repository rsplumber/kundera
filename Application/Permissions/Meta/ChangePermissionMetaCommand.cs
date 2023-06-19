using System.Collections.Immutable;
using Core.Permissions;
using Core.Permissions.Exceptions;
using Mediator;

namespace Application.Permissions.Meta;

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
        var permission = await _permissionRepository.FindAsync(command.PermissionId, cancellationToken);

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