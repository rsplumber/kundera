using Domain;
using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

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