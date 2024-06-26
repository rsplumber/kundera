﻿using Core.Scopes;
using Core.Scopes.Exceptions;
using Mediator;

namespace Application.Scopes.Status.DeActivate;

public sealed record DeActivateScopeCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;
}

internal sealed class DeActivateScopeCommandHandler : ICommandHandler<DeActivateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeActivateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(DeActivateScopeCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(command.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.DeActivate();

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}