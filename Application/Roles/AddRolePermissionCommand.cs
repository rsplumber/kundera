using Core.Domains.Permissions;
using Core.Domains.Permissions.Exceptions;
using Core.Domains.Permissions.Types;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Roles.Types;
using FluentValidation;
using Mediator;

namespace Application.Roles;

public sealed record AddRolePermissionCommand : ICommand
{
    public Guid Role { get; init; } = default!;

    public Guid[] Permissions { get; init; } = default!;
}

internal sealed class AddRolePermissionCommandHandler : ICommandHandler<AddRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AddRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(AddRolePermissionCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(RoleId.From(command.Role), cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permissionId in command.Permissions)
        {
            var permission = await _permissionRepository.FindAsync(PermissionId.From(permissionId), cancellationToken);
            if (permission is null)
            {
                throw new PermissionNotFoundException();
            }

            role.AddPermission(permission.Id);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}

public sealed class AddRolePermissionCommandValidator : AbstractValidator<AddRolePermissionCommand>
{
    public AddRolePermissionCommandValidator()
    {
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter a Role")
            .NotNull().WithMessage("Enter a Role");

        RuleFor(request => request.Permissions)
            .NotEmpty().WithMessage("Enter a at least one permission")
            .NotNull().WithMessage("Enter a at least one permission");
    }
}