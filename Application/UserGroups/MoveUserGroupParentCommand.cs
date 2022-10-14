using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId UserGroupId, UserGroupId To) : Command;

internal sealed class MoveUserGroupParentCommandHandler : ICommandHandler<MoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public MoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(MoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
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