using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ActivateScopeCommand(ScopeId Scope) : Command;

internal sealed class ActivateScopeCommandHandler : CommandHandler<ActivateScopeCommand>
{
    public override async Task HandleAsync(ActivateScopeCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}