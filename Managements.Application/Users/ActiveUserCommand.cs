using FluentValidation;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record ActiveUserCommand : ICommand
{
    public Guid User { get; init; } = default!;
}

internal sealed class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActiveUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async ValueTask<Unit> Handle(ActiveUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Activate();

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class ActiveUserCommandValidator : AbstractValidator<ActiveUserCommand>
{
    public ActiveUserCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");
    }
}