using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record DeActivateScopeCommand(ScopeId Scope) : Command;

internal sealed class DeActivateScopeCommandHandler : CommandHandler<DeActivateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeActivateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(DeActivateScopeCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        var scope = await _scopeRepository.FindAsync(message.Scope, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.DeActivate();
    }
}