using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using FluentValidation;
using Mediator;

namespace Application.Scopes;

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
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        scope.DeActivate();

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeActivateScopeCommandValidator : AbstractValidator<DeActivateScopeCommand>
{
    public DeActivateScopeCommandValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");
    }
}