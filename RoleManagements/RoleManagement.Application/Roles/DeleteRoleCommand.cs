using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record DeleteRoleCommand(RoleId Id) : Command;

internal sealed class DeleteRoleCommandHandler : CommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async override Task HandleAsync(DeleteRoleCommand message, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.FindAsync(message.Id, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteAsync(role.Id, cancellationToken);
    }
}