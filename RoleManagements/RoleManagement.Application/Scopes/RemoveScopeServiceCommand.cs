using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record RemoveScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class RemoveScopeServiceCommandHandler : CommandHandler<RemoveScopeServiceCommand>
{
    public override async Task HandleAsync(RemoveScopeServiceCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}