using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using Mediator;

namespace Application.Groups;

public sealed record MoveGroupParentCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid To { get; init; } = default!;
}

internal sealed class MoveGroupParentCommandHandler : ICommandHandler<MoveGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public MoveGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }


    public async ValueTask<Unit> Handle(MoveGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.GroupId), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(GroupId.From(command.To), cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return Unit.Value;
    }
}