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
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(message.Parent);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}