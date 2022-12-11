using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record RemoveGroupParentCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;
}

internal sealed class RemoveGroupParentCommandHandler : ICommandHandler<RemoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RemoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(RemoveGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.GroupId), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.RemoveParent();
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return Unit.Value;
    }
}

public sealed class RemoveGroupParentCommandValidator : AbstractValidator<RemoveGroupParentCommand>
{
    public RemoveGroupParentCommandValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a group id")
            .NotNull().WithMessage("Enter a group id");
    }
}