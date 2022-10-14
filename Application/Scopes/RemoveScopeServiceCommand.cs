﻿using Domain.Scopes;
using Domain.Scopes.Exceptions;
using Domain.Services;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Scopes;

public sealed record RemoveScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class RemoveScopeServiceCommandHandler : ICommandHandler<RemoveScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeServiceCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask HandleAsync(RemoveScopeServiceCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, serviceIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var service in serviceIds)
        {
            scope.RemoveService(service);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}