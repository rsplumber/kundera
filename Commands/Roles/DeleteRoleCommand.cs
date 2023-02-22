using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;

namespace Commands.Roles;

public sealed record DeleteRoleCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;
}

internal sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteAsync(role.Id, cancellationToken);

        return Unit.Value;
    }
}