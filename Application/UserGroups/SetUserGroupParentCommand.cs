using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

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