using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Domain.Users;
using Domain.Users.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record JoinUserToGroupCommand(UserId User, UserGroupId UserGroup) : Command;

internal sealed class JoinUserToGroupCommandHandler : CommandHandler<JoinUserToGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserGroupRepository _userGroupRepository;

    public JoinUserToGroupCommandHandler(IUserRepository userRepository, IUserGroupRepository userGroupRepository)
    {
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(JoinUserToGroupCommand message, CancellationToken cancellationToken = default)
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

        user.JoinGroup(userGroupId);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}