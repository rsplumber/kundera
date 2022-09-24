using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record RemoveScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class RemoveScopeRoleCommandHandler : CommandHandler<RemoveScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeRoleCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(RemoveScopeRoleCommand message, CancellationToken cancellationToken = default)
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
    }
}