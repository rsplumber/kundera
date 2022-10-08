using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class DisableUserGroupCommandHandler : CommandHandler<DisableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public DisableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(DisableUserGroupCommand message, CancellationToken cancellationToken = default)
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