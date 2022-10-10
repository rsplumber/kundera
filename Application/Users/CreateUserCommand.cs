using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Domain.Users;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record CreateUserCommand(Username Username, UserGroupId UserGroup) : Command;

internal sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserGroupRepository _userGroupRepository;

    public CreateUserCommandHandler(IUserRepository userRepository, IUserGroupRepository userGroupRepository)
    {
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(CreateUserCommand message, CancellationToken cancellationToken = default)
    {
        var (username, userGroupId) = message;
        var userGroup = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (userGroup is null)
        {
            throw new UserGroupNotFoundException();
        }

        var user = await User.CreateAsync(username, userGroupId, _userRepository);
        await _userRepository.AddAsync(user, cancellationToken);
    }
}