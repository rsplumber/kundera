using Core.Groups;
using Core.Groups.Exception;
using Mediator;

namespace Application.Groups.Status.Disable;

public sealed record DisableGroupCommand : ICommand
{
    public Guid GroupId { get; set; } = default!;
}

internal sealed class DisableGroupCommandHandler : ICommandHandler<DisableGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DisableGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(DisableGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.Disable();
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}