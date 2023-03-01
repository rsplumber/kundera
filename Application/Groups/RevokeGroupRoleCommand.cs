using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Mediator;

namespace Application.Groups;

public sealed record RevokeGroupRoleCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class RevokeGroupRoleCommandHandler : ICommandHandler<RevokeGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RevokeGroupRoleCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(RevokeGroupRoleCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var roleId in command.RolesIds)
        {
            group.RevokeRole(roleId);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}