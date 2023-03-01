using Core.Domains.Permissions;
using Mediator;

namespace Application.Permissions;

public sealed record CreatePermissionCommand : ICommand<Permission>
{
    public string Name { get; init; } = default!;

    public IDictionary<string, string>? Meta { get; init; }
}

internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, Permission>
{
    private readonly IPermissionFactory _permissionFactory;

    public CreatePermissionCommandHandler(IPermissionFactory permissionFactory)
    {
        _permissionFactory = permissionFactory;
    }

    public async ValueTask<Permission> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        var permission = await _permissionFactory.CreateAsync(command.Name, command.Meta);
        return permission;
    }
}