using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record DeActivateScopeCommand(ScopeId Scope) : Command;

internal sealed class DeActivateScopeCommandHandler : CommandHandler<DeActivateScopeCommand>
{
    public override async Task HandleAsync(DeActivateScopeCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}