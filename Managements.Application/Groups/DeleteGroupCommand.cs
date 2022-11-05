using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;

namespace Managements.Application.Groups;

public sealed record DeleteGroupCommand(GroupId Id) : Command;

internal sealed class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DeleteGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(DeleteGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.FindAsync(message.Id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteAsync(group.Id, cancellationToken);
    }
}