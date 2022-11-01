using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Roles;

namespace Managements.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly IRoleFactory _roleFactory;

    public CreateRoleCommandHandler(IRoleFactory roleFactory)
    {
        _roleFactory = roleFactory;
    }

    public async Task HandleAsync(CreateRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        await _roleFactory.CreateAsync(name, meta);
    }
}