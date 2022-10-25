using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record JoinUserToGroupCommand(UserId User, GroupId Group) : Command;

internal sealed class JoinUserToGroupCommandHandler : ICommandHandler<JoinUserToGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public JoinUserToGroupCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(JoinUserToGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, groupId) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        user.JoinGroup(groupId);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}