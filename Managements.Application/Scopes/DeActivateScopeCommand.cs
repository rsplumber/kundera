﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;

namespace Managements.Application.Scopes;

public sealed record DeActivateScopeCommand(ScopeId Scope) : Command;

internal sealed class DeActivateScopeCommandHandler : ICommandHandler<DeActivateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeActivateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async Task HandleAsync(DeActivateScopeCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        var scope = await _scopeRepository.FindAsync(message.Scope, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.DeActivate();
        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}