using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record RemoveUserGroupParentCommand(UserGroupId UserGroup) : Command;

internal sealed class RemoveUserGroupParentCommandHandler : CommandHandler<RemoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public RemoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(RemoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
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