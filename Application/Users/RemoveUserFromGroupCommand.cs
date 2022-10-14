using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Domain.Users;
using Domain.Users.Exception;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Users;

public sealed record RemoveUserFromGroupCommand(UserId User, UserGroupId UserGroup) : Command;

internal sealed class RemoveUserFromGroupCommandHandler : ICommandHandler<RemoveUserFromGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserGroupRepository _userGroupRepository;

    public RemoveUserFromGroupCommandHandler(IUserRepository userRepository, IUserGroupRepository userGroupRepository)
    {
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(RemoveUserFromGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, userGroupId) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var userGroup = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (userGroup is null)
        {
            throw new UserGroupNotFoundException();
        }

        user.RemoveFromGroup(userGroupId);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}