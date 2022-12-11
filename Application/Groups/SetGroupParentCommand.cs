using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record SetGroupParentCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid Parent { get; init; } = default!;
}

internal sealed class SetGroupParentCommandHandler : ICommandHandler<SetGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public SetGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(SetGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.GroupId), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(GroupId.From(command.Parent), cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class SetGroupParentCommandValidator : AbstractValidator<SetGroupParentCommand>
{
    public SetGroupParentCommandValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.Parent)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}