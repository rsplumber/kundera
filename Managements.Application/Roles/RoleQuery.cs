using FluentValidation;
using Mediator;

namespace Managements.Application.Roles;

public sealed record RoleQuery : IQuery<RoleResponse>
{
    public Guid Role { get; init; } = default!;
}

public sealed record RoleResponse(Guid Id, string Name)
{
    public IEnumerable<Guid>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}

public sealed class RoleQueryValidator : AbstractValidator<RoleQuery>
{
    public RoleQueryValidator()
    {
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter Role")
            .NotNull().WithMessage("Enter Role");
    }
}