using Core.Domains.Roles.Types;
using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Core.Domains.Users.Types;
using FluentValidation;
using Mediator;

namespace Application.Users;

public sealed record AssignUserRoleCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class AssignUserRoleCommandHandler : ICommandHandler<AssignUserRoleCommand>
{
    private readonly IUserRepository _userRepository;

    public AssignUserRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(AssignUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.UserId), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        foreach (var role in command.Roles)
        {
            user.AssignRole(RoleId.From(role));
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter a User")
            .NotNull().WithMessage("Enter a User");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}