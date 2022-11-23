using FluentValidation;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record ActivateScopeCommand : ICommand
{
    public Guid Scope { get; init; } = default!;
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
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
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
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter Scope")
            .NotNull().WithMessage("Enter Scope");
    }
}