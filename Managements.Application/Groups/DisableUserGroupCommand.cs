using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record DisableGroupCommand(GroupId Group) : Command;

internal sealed class DisableGroupCommandHandler : ICommandHandler<DisableGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DisableGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(DisableGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.FindAsync(message.Group, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.Disable();
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}