using FluentValidation;
using Mediator;

namespace Application.Users;

public sealed record UserQuery : IQuery<UserResponse>
{
    public Guid User { get; init; } = default!;
}

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Status { get; init; } = string.Empty;

    public List<string> Usernames { get; init; } = Array.Empty<string>().ToList();

    public List<Guid> Groups { get; init; } = Array.Empty<Guid>().ToList();

    public List<Guid>? Roles { get; init; } = Array.Empty<Guid>().ToList();
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