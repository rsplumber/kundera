using RoleManagements.Domain;
using RoleManagements.Domain.Permissions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

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
        var permission = await Permission.CreateAsync(message.Name, _permissionRepository);
        await _permissionRepository.AddAsync(permission, cancellationToken);
    }
}