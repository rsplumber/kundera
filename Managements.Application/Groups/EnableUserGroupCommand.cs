using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record EnableGroupCommand(GroupId Id) : Command;

internal sealed class EnableGroupCommandHandler : ICommandHandler<EnableGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public EnableGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(EnableGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.FindAsync(message.Id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.Enable();
        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}