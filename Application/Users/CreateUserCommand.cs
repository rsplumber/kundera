using Domain.UserGroups;
using Domain.Users;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record CreateUserCommand(Username Username, UserGroupId UserGroup) : Command;

internal sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(CreateUserCommand message, CancellationToken cancellationToken = default)
    {
        var (username, userGroupId) = message;
        var user = await User.CreateAsync(username, userGroupId, _userRepository);
        await _userRepository.AddAsync(user, cancellationToken);
    }
}