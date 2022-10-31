using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Roles;

namespace Managements.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly IRoleFactory _roleFactory;
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository, IRoleFactory roleFactory)
    {
        _roleRepository = roleRepository;
        _roleFactory = roleFactory;
    }

    public async Task HandleAsync(CreateRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        var role = await _roleFactory.CreateAsync(name);

        await _roleRepository.AddAsync(role, cancellationToken);
    }
}