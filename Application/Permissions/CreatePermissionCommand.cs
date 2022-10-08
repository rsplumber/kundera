using Domain;
using Domain.Permissions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Permissions;

public sealed record CreatePermissionCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreatePermissionCommandHandler : CommandHandler<CreatePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public override async Task HandleAsync(CreatePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        var permission = await Permission.FromAsync(name, _permissionRepository);
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                permission.AddMeta(key, value);
            }
        }

        await _permissionRepository.AddAsync(permission, cancellationToken);
    }
}