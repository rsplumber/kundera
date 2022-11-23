using FluentValidation;
using Managements.Domain.Roles.Types;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record RevokeUserRoleCommand : ICommand
{
    public Guid User { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class RevokeUserRoleCommandHandler : ICommandHandler<RevokeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public RevokeUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(RevokeUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in command.Roles)
        {
            user.RevokeRole(RoleId.From(role));
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RevokeUserRoleCommandValidator : AbstractValidator<RevokeUserRoleCommand>
{
    public RevokeUserRoleCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a User")
            .NotNull().WithMessage("Enter a User");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}