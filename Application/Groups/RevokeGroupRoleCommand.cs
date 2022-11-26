using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using Core.Domains.Roles.Types;
using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record RevokeGroupRoleCommand : ICommand
{
    public Guid Group { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class RevokeGroupRoleCommandHandler : ICommandHandler<RevokeGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RevokeGroupRoleCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(RevokeGroupRoleCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var role in command.Roles)
        {
            group.RevokeRole(RoleId.From(role));
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RevokeGroupRoleCommandValidator : AbstractValidator<RevokeGroupRoleCommand>
{
    public RevokeGroupRoleCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}