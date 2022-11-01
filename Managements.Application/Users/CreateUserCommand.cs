using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Users;

namespace Managements.Application.Users;

public sealed record CreateUserCommand(Username Username, GroupId Group) : Command;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserFactory _userFactory;

    public CreateUserCommandHandler(IUserFactory userFactory)
    {
        _userFactory = userFactory;
    }

    public async Task HandleAsync(CreateUserCommand message, CancellationToken cancellationToken = default)
    {
        var (username, groupId) = message;
        await _userFactory.CreateAsync(username, groupId);
    }
}