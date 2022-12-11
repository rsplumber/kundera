﻿using Core.Domains.Roles.Types;
using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Core.Domains.Users.Types;
using FluentValidation;
using Mediator;

namespace Application.Users;

public sealed record RevokeUserRoleCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

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
        var user = await _userRepository.FindAsync(UserId.From(command.UserId), cancellationToken);
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
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter a User")
            .NotNull().WithMessage("Enter a User");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}