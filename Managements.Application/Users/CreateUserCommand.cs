using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Users;

namespace Managements.Application.Users;

public sealed record CreateUserCommand(Username Username, GroupId Group) : Command;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public CreateUserCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(CreateUserCommand message, CancellationToken cancellationToken = default)
    {
        var (username, groupId) = message;
        var group = await _groupRepository.FindAsync(message.Group, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var user = await User.CreateAsync(username, groupId, _userRepository);
        await _userRepository.AddAsync(user, cancellationToken);
    }
}