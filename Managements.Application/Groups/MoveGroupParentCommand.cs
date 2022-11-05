using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record MoveGroupParentCommand(GroupId GroupId, GroupId To) : Command;

internal sealed class MoveGroupParentCommandHandler : ICommandHandler<MoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public MoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(MoveGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, to) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(to, cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}