using FluentValidation;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record AddUserUsernameCommand : ICommand
{
    public Guid User { get; init; } = default!;

    public string Username { get; init; } = default!;
}

internal sealed class AddUserUsernameCommandHandler : ICommandHandler<AddUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public AddUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(AddUserUsernameCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.AddUsername(command.Username);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class AddUserUsernameCommandValidator : AbstractValidator<AddUserUsernameCommand>
{
    public AddUserUsernameCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a User")
            .NotNull().WithMessage("Enter a User");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter a Username")
            .NotNull().WithMessage("Enter a Username");
    }
}