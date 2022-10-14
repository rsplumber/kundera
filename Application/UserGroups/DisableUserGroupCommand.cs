using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class DisableUserGroupCommandHandler : ICommandHandler<EnableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public DisableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(EnableUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.Enable();
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}