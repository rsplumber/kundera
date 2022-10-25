using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record ChangeGroupNameCommand(GroupId Group, Name Name) : Command;

internal sealed class ChangeGroupNameCommandHandler : ICommandHandler<ChangeGroupNameCommand>
{
    private readonly IGroupRepository _groupRepository;

    public ChangeGroupNameCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(ChangeGroupNameCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, name) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.ChangeName(name);
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}