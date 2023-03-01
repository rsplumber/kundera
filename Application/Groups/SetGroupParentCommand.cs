using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Mediator;

namespace Application.Groups;

public sealed record SetGroupParentCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid ParentId { get; init; } = default!;
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
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(command.ParentId, cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}