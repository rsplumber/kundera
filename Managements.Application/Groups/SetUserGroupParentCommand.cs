using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record SetGroupParentCommand(GroupId Group, GroupId Parent) : Command;

internal sealed class SetGroupParentCommandHandler : ICommandHandler<SetGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public SetGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(SetGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, parentId) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(parentId, cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}