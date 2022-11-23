using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Mediator;

namespace Managements.Application.Groups;

public sealed record MoveGroupParentCommand : ICommand
{
    public Guid Group { get; init; } = default!;

    public Guid To { get; init; } = default!;
}

internal sealed class MoveGroupParentCommandHandler : ICommandHandler<MoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public MoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }


    public async ValueTask<Unit> Handle(MoveGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(GroupId.From(command.To), cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return Unit.Value;
    }
}

public sealed class MoveGroupParentCommandValidator : AbstractValidator<MoveGroupParentCommand>
{
    public MoveGroupParentCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.To)
            .NotEmpty().WithMessage("Enter a To")
            .NotNull().WithMessage("Enter a To");
    }
}