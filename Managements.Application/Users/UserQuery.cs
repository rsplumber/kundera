using FluentValidation;
using Mediator;

namespace Managements.Application.Users;

public sealed record UserQuery : IQuery<UserResponse>
{
    public Guid User { get; init; } = default!;
}

public sealed record UserResponse()
{
    public Guid Id { get; set; }

    public string Status { get; set; }

    public List<string> Usernames { get; set; } = Array.Empty<string>().ToList();

    public List<Guid> Groups { get; set; } = Array.Empty<Guid>().ToList();

    public List<Guid> Roles { get; set; } = Array.Empty<Guid>().ToList();
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