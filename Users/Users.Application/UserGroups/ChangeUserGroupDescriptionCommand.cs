using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupDescriptionCommand(UserGroupId UserGroup, Text Description) : Command;

internal sealed class ChangeUserGroupDescriptionCommandHandler : CommandHandler<ChangeUserGroupDescriptionCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public ChangeUserGroupDescriptionCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(ChangeUserGroupDescriptionCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.ChangeDescription(message.Description);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}