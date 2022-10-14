using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class EnableUserGroupCommandHandler : ICommandHandler<DisableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public EnableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(DisableUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.Disable();
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}