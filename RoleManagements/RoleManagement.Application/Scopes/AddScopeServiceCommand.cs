using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class AddScopeServiceCommandHandler : CommandHandler<AddScopeServiceCommand>
{
    public override async Task HandleAsync(AddScopeServiceCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}