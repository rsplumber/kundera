using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Users.Application.Users;

public sealed record JoinUserToGroupCommand(UserId User, UserGroupId UserGroup) : Command;

public sealed record RemoveUserFromGroupCommand(UserId User, UserGroupId UserGroup) : Command;

internal sealed class JoinUserToGroupCommandHandler : CommandHandler<JoinUserToGroupCommand>
{
    private readonly IUserRepository _userRepository;

    public JoinUserToGroupCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(JoinUserToGroupCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.JoinGroup(message.UserGroup);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}

internal sealed class RemoveUserFromGroupCommandHandler : CommandHandler<RemoveUserFromGroupCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserFromGroupCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(RemoveUserFromGroupCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveFromGroup(message.UserGroup);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}