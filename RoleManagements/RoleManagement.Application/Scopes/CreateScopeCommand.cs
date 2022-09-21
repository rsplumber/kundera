using RoleManagements.Domain;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : CommandHandler<CreateScopeCommand>
{
    public override async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}