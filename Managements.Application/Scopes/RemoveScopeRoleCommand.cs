using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;

namespace Managements.Application.Scopes;

public sealed record RemoveScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class RemoveScopeRoleCommandHandler : ICommandHandler<RemoveScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeRoleCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async Task HandleAsync(RemoveScopeRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, roleIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var role in roleIds)
        {
            scope.RemoveRole(role);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}