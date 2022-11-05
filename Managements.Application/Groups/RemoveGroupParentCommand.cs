using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record RemoveGroupParentCommand(GroupId Id) : Command;

internal sealed class RemoveGroupParentCommandHandler : ICommandHandler<RemoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RemoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(RemoveGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.FindAsync(message.Id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.RemoveParent();
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}