using Domain.Scopes;
using Domain.Scopes.Exceptions;
using Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ActivateScopeCommand(ScopeId Scope) : Command;

internal sealed class ActivateScopeCommandHandler : CommandHandler<ActivateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public ActivateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(ActivateScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await _scopeRepository.FindAsync(message.Scope, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.Activate();

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}