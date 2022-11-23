using FluentValidation;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Roles.Types;
using Mediator;

namespace Managements.Application.Roles;

public sealed record DeleteRoleCommand : ICommand
{
    public Guid Role { get; init; } = default!;
}

internal sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(RoleId.From(command.Role), cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteAsync(role.Id, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter a Role")
            .NotNull().WithMessage("Enter a Role");
    }
}