using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record RemoveUserGroupParentCommand(UserGroupId UserGroup) : Command;

internal sealed class RemoveUserGroupParentCommandHandler : ICommandHandler<RemoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public RemoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(RemoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.RemoveParent();
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}