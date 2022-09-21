using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record SetUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;

internal sealed class SetUserGroupParentCommandHandler : ICommandHandler<SetUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public SetUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(SetUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, parent) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(parent);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}