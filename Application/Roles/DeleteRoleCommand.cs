using Domain.Roles;
using Domain.Roles.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Roles;

public sealed record DeleteRoleCommand(RoleId Id) : Command;

internal sealed class DeleteRoleCommandHandler : CommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(DeleteRoleCommand message, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.FindAsync(message.Id, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteAsync(role.Id, cancellationToken);
    }
}