using Core.Domains.Groups.Types;
using Core.Domains.Users;
using Mediator;

namespace Application.Users;

public sealed record CreateUserCommand : ICommand<User>
{
    public string Username { get; init; } = default!;

    public Guid Group { get; set; } = default!;
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
        var user = await _userFactory.CreateAsync(command.Username, GroupId.From(command.Group));

        return user;
    }
}