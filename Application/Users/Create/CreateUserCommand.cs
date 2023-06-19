using Core.Users;
using Mediator;

namespace Application.Users.Create;

public sealed record CreateUserCommand : ICommand<User>
{
    public string Username { get; init; } = default!;

    public Guid GroupId { get; set; } = default!;
}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly IUserFactory _userFactory;

    public CreateUserCommandHandler(IUserFactory userFactory)
    {
        _userFactory = userFactory;
    }

    public async ValueTask<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userFactory.CreateAsync(command.Username, command.GroupId);

        return user;
    }
}