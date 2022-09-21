using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.Users;

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
        var user = await User.CreateAsync(message.Username, message.UserGroup, _userRepository);
        await _userRepository.AddAsync(user, cancellationToken);
    }
}