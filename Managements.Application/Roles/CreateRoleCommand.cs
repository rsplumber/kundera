using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Roles;

namespace Managements.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(CreateRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        var role = await Role.FromAsync(name, _roleRepository);
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                role.AddMeta(key, value);
            }
        }

        await _roleRepository.AddAsync(role, cancellationToken);
    }
}