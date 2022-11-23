using FluentValidation;
using Managements.Domain.Roles.Types;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record RemoveScopeRoleCommand : ICommand
{
    public Guid Scope { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class RemoveScopeRoleCommandHandler : ICommandHandler<RemoveScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeRoleCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(RemoveScopeRoleCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var role in command.Roles)
        {
            scope.RemoveRole(RoleId.From(role));
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RemoveScopeRoleCommandValidator : AbstractValidator<RemoveScopeRoleCommand>
{
    public RemoveScopeRoleCommandValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter at least one role")
            .NotNull().WithMessage("Enter at least one role");
    }
}