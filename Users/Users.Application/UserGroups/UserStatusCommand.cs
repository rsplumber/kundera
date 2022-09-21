using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroup) : Command;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;

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

internal sealed class EnableUserGroupCommandHandler : CommandHandler<EnableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public EnableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(EnableUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.Enable();
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}