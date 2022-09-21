using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record RemoveScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class RemoveScopeRoleCommandHandler : CommandHandler<RemoveScopeRoleCommand>
{
    public override async Task HandleAsync(RemoveScopeRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}