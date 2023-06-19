using Core.Groups;
using Core.Groups.Exception;
using Core.Users;
using Core.Users.Exception;
using Mediator;

namespace Application.Users.Groups.Leave;

public sealed record LeaveUserFromGroupCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid GroupId { get; init; } = default!;
}

internal sealed class LeaveUserFromGroupCommandHandler : ICommandHandler<LeaveUserFromGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public LeaveUserFromGroupCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(LeaveUserFromGroupCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        user.Leave(group);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}