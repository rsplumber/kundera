using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record ChangeGroupDescriptionCommand(GroupId Group, Text Description) : Command;

internal sealed class ChangeGroupDescriptionCommandHandler : ICommandHandler<ChangeGroupDescriptionCommand>
{
    private readonly IGroupRepository _groupRepository;

    public ChangeGroupDescriptionCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(ChangeGroupDescriptionCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, description) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.ChangeDescription(description);
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}