using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record RemoveUserFromGroupCommand(UserId User, GroupId Group) : Command;

internal sealed class RemoveUserFromGroupCommandHandler : ICommandHandler<RemoveUserFromGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public RemoveUserFromGroupCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(RemoveUserFromGroupCommand message, CancellationToken cancellationToken = default)
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

        user.RemoveFromGroup(groupId);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}