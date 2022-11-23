using FluentValidation;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record RemoveScopeServiceCommand : ICommand
{
    public Guid Scope { get; init; } = default!;

    public Guid[] Services { get; init; } = default!;
}

internal sealed class RemoveScopeServiceCommandHandler : ICommandHandler<RemoveScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeServiceCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(RemoveScopeServiceCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var service in command.Services)
        {
            scope.RemoveService(ServiceId.From(service));
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RemoveScopeServiceCommandValidator : AbstractValidator<RemoveScopeServiceCommand>
{
    public RemoveScopeServiceCommandValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.Services)
            .NotEmpty().WithMessage("Enter at least one service")
            .NotNull().WithMessage("Enter at least one service");
    }
}