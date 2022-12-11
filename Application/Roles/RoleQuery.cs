using FluentValidation;
using Mediator;

namespace Application.Roles;

public sealed record RoleQuery : IQuery<RoleResponse>
{
    public Guid RoleId { get; init; } = default!;
}

public sealed record RoleResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<Guid>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}

public sealed class RoleQueryValidator : AbstractValidator<RoleQuery>
{
    public RoleQueryValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter Role")
            .NotNull().WithMessage("Enter Role");
    }
}