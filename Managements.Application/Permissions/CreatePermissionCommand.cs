using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Permissions;

namespace Managements.Application.Permissions;

public sealed record CreatePermissionCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand>
{
    private readonly IPermissionFactory _permissionFactory;

    public CreatePermissionCommandHandler(IPermissionFactory permissionFactory)
    {
        _permissionFactory = permissionFactory;
    }

    public async Task HandleAsync(CreatePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        await _permissionFactory.CreateAsync(name, meta);
    }
}