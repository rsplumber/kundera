using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRoleMetaCommand(RoleId Role, IDictionary<string, string> Meta) : Command;

internal sealed class AddRoleMetaCommandHandler : CommandHandler<AddRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public AddRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(AddRoleMetaCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, dictionary) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var (key, value) in dictionary)
        {
            role.AddMeta(key, value);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
    }
}