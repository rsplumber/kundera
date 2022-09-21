using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class AddScopeRoleCommandHandler : CommandHandler<AddScopeRoleCommand>
{
    public override async Task HandleAsync(AddScopeRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}