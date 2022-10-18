using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Managements.Application.Users;

public sealed record RevokeUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class RevokeUserRoleCommandHandler : ICommandHandler<RevokeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public RevokeUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(RevokeUserRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (userId, roleIds) = message;
        var user = await _userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in roleIds)
        {
            user.RevokeRole(role);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}