using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record DisableGroupCommand : ICommand
{
    public Guid GroupId { get; set; } = default!;
}

internal sealed class DisableGroupCommandHandler : ICommandHandler<DisableGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DisableGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(DisableGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.GroupId), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.Disable();
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DisableGroupCommandValidator : AbstractValidator<DisableGroupCommand>
{
    public DisableGroupCommandValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}