using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Mediator;

namespace Application.Scopes;

public sealed record RemoveScopeRoleCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class RemoveScopeRoleCommandHandler : ICommandHandler<RemoveScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public RemoveScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(RemoveScopeRoleCommand command, CancellationToken cancellationToken)
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