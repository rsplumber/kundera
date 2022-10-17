using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record ChangeUserGroupDescriptionCommand(UserGroupId UserGroup, Text Description) : Command;

internal sealed class ChangeUserGroupDescriptionCommandHandler : ICommandHandler<ChangeUserGroupDescriptionCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public ChangeUserGroupDescriptionCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(ChangeUserGroupDescriptionCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, description) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.ChangeDescription(description);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}