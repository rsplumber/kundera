using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Roles;

public sealed record AddRoleMetaCommand(RoleId Role, IDictionary<string, string> Meta) : Command;

internal sealed class AddRoleMetaCommandHandler : ICommandHandler<AddRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public AddRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(AddRoleMetaCommand message, CancellationToken cancellationToken = default)
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