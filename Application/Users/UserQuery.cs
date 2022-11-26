using FluentValidation;
using Mediator;

namespace Application.Users;

public sealed record UserQuery : IQuery<UserResponse>
{
    public Guid User { get; init; } = default!;
}

public sealed record UserResponse(Guid Id, IEnumerable<string> Usernames)
{
    public string Status { get; set; }

    public IEnumerable<Guid> Groups { get; set; } = Array.Empty<Guid>();

    public IEnumerable<Guid> Roles { get; set; } = Array.Empty<Guid>();
}

public sealed class UserQueryValidator : AbstractValidator<UserQuery>
{
    public UserQueryValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");
    }
}