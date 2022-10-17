using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record ChangeUserGroupNameCommand(UserGroupId UserGroup, Name Name) : Command;

internal sealed class ChangeUserGroupNameCommandHandler : ICommandHandler<ChangeUserGroupNameCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public ChangeUserGroupNameCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(ChangeUserGroupNameCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, name) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.ChangeName(name);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}