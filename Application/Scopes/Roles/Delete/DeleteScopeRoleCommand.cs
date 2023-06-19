using Core.Roles;
using Core.Roles.Exceptions;
using Core.Scopes;
using Core.Scopes.Exceptions;
using Mediator;

namespace Application.Scopes.Roles.Delete;

public sealed record DeleteScopeRoleCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class DeleteScopeRoleCommandHandler : ICommandHandler<DeleteScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public DeleteScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(DeleteScopeRoleCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(command.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var roleId in command.RolesIds)
        {
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }
            scope.Remove(role);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}