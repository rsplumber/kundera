﻿using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using FluentValidation;
using Mediator;

namespace Application.Scopes;

public sealed record ActivateScopeCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;
}

internal sealed class ActivateScopeCommandHandler : ICommandHandler<ActivateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public ActivateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(ActivateScopeCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.Activate();

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}

public sealed class ActivateScopeCommandValidator : AbstractValidator<ActivateScopeCommand>
{
    public ActivateScopeCommandValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter Scope")
            .NotNull().WithMessage("Enter Scope");
    }
}