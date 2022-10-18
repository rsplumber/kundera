using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class DisableUserGroupCommandHandler : ICommandHandler<EnableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public DisableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(EnableUserGroupCommand message, CancellationToken cancellationToken = default)
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