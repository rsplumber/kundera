using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Roles.Types;
using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record AssignGroupRoleCommand : ICommand
{
    public Guid Group { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class AssignGroupRoleCommandHandler : ICommandHandler<AssignGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;

    public AssignGroupRoleCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(AssignGroupRoleCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var roleId in command.Roles)
        {
            var role = await _roleRepository.FindAsync(RoleId.From(roleId), cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            group.AssignRole(role.Id);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class AssignGroupRoleCommandValidator : AbstractValidator<AssignGroupRoleCommand>
{
    public AssignGroupRoleCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}