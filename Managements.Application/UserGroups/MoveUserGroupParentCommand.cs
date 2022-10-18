using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId UserGroupId, UserGroupId To) : Command;

internal sealed class MoveUserGroupParentCommandHandler : ICommandHandler<MoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public MoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(MoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, to) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        var parent = await _userGroupRepository.FindAsync(to, cancellationToken);
        if (parent is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}