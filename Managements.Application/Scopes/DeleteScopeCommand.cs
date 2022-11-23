using FluentValidation;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record DeleteScopeCommand : ICommand
{
    public Guid Scope { get; init; } = default!;
}

internal sealed class DeleteScopeCommandHandler : ICommandHandler<DeleteScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeleteScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(DeleteScopeCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        await _scopeRepository.DeleteAsync(scope.Id, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeleteScopeCommandValidator : AbstractValidator<DeleteScopeCommand>
{
    public DeleteScopeCommandValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");
    }
}