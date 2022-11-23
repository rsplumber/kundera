using FluentValidation;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Roles.Types;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record AddScopeRoleCommand : ICommand
{
    public Guid Scope { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class AddScopeRoleCommandHandler : ICommandHandler<AddScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public AddScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(AddScopeRoleCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var roleId in command.Roles)
        {
            var role = await _roleRepository.FindAsync(RoleId.From(roleId), cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            scope.AddRole(role.Id);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}

public sealed class AddScopeRoleCommandValidator : AbstractValidator<AddScopeRoleCommand>
{
    public AddScopeRoleCommandValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter at least one role")
            .NotNull().WithMessage("Enter at least one role");
    }
}