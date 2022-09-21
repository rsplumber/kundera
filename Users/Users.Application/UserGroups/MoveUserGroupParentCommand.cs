using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId From, UserGroupId To) : Command;

internal sealed class MoveUserGroupParentCommandHandler : CommandHandler<MoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public MoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(MoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.From, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(message.To);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}