using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record AssignUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class AssignUserRoleCommandHandler : ICommandHandler<AssignUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public AssignUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(AssignUserRoleCommand message, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync(message.User, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in message.Roles)
        {
            user.AssignRole(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}