using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Users.Application.Users;

public sealed record AssignUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class AssignUserRoleCommandHandler : CommandHandler<AssignUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public AssignUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task HandleAsync(AssignUserRoleCommand message, CancellationToken cancellationToken = default)
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