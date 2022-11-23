using FluentValidation;
using Managements.Domain.Groups.Types;
using Managements.Domain.Users;
using Mediator;

namespace Managements.Application.Users;

public sealed record CreateUserCommand : ICommand<User>
{
    public string Username { get; init; } = default!;

    public Guid Group { get; set; } = default!;
}

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly IUserFactory _userFactory;

    public CreateUserCommandHandler(IUserFactory userFactory)
    {
        _userFactory = userFactory;
    }

    public async ValueTask<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userFactory.CreateAsync(command.Username, GroupId.From(command.Group));

        return user;
    }
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}