using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Mediator;

namespace Application.Groups;

public sealed record RemoveGroupParentCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;
}

internal sealed class RemoveGroupParentCommandHandler : ICommandHandler<RemoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RemoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(RemoveGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.RemoveParent();
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return Unit.Value;
    }
}