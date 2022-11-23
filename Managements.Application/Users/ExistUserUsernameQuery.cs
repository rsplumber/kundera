using FluentValidation;
using Mediator;

namespace Managements.Application.Users;

public sealed record ExistUserUsernameQuery : IQuery<Guid>
{
    public string Username { get; init; } = default!;
}

public sealed class ExistUserUsernameQueryValidator : AbstractValidator<ExistUserUsernameQuery>
{
    public ExistUserUsernameQueryValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");
    }
}