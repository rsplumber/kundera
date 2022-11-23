using FluentValidation;
using Managements.Domain.Permissions.Types;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Roles.Types;
using Mediator;

namespace Managements.Application.Roles;

public sealed record RemoveRolePermissionCommand : ICommand
{
    public Guid Role { get; init; } = default!;

    public Guid[] Permissions { get; init; } = default!;
}

internal sealed class RemoveRolePermissionCommandHandler : ICommandHandler<RemoveRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRolePermissionCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(RemoveRolePermissionCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(RoleId.From(command.Role), cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permission in command.Permissions)
        {
            role.RemovePermission(PermissionId.From(permission));
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RemoveRolePermissionCommandValidator : AbstractValidator<RemoveRolePermissionCommand>
{
    public RemoveRolePermissionCommandValidator()
    {
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter a Role")
            .NotNull().WithMessage("Enter a Role");

        RuleFor(request => request.Permissions)
            .NotEmpty().WithMessage("Enter a at least one permission")
            .NotNull().WithMessage("Enter a at least one permission");
    }
}