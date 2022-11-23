using FluentValidation;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record RemoveUserUsernameCommand : ICommand
{
    public Guid User { get; init; } = default!;

    public string Username { get; init; } = default!;
}

internal sealed class RemoveUserUsernameCommandHandler : ICommandHandler<RemoveUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(RemoveUserUsernameCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveUsername(command.Username);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class RemoveUserUsernameCommandValidator : AbstractValidator<RemoveUserUsernameCommand>
{
    public RemoveUserUsernameCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");
    }
}