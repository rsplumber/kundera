using Core.Users;
using Mediator;

namespace Application.Users.Create;

public sealed record CreateUserCommand : ICommand<User>
{
    public Guid? UserId { get; set; } = default!;

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
        if (command.UserId is not null)
        {
            return await _userFactory.CreateAsync(command.UserId.Value, command.GroupId);
        }
        else
        {
            return await _userFactory.CreateAsync(command.GroupId);
        }
    }
}