using RoleManagements.Domain;
using RoleManagements.Domain.Roles;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreateRoleCommandHandler : CommandHandler<CreateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(CreateRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        var role = await Role.CreateAsync(name, _roleRepository);
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                role.AddMeta(key,value);
            }
        }
        await _roleRepository.AddAsync(role, cancellationToken);
    }
}