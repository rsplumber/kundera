using FluentValidation;
using Mediator;

namespace Application.Roles;

public sealed record RolePermissionsQuery : IQuery<List<RolePermissionsResponse>>
{
    public Guid Role { get; init; } = default!;
}

public sealed record RolePermissionsResponse(Guid Id, string Name);

public sealed class RolePermissionsQueryValidator : AbstractValidator<RolePermissionsQuery>
{
    public RolePermissionsQueryValidator()
    {
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter Role")
            .NotNull().WithMessage("Enter Role");
    }
}