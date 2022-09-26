using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class AddScopeRoleCommandHandler : CommandHandler<AddScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public AddScopeRoleCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(AddScopeRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, roleIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var role in roleIds)
        {
            scope.AddRole(role);
        }
        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}