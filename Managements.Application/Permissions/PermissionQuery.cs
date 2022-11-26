using FluentValidation;
using Managements.Domain;
using Mediator;

namespace Managements.Application.Permissions;

public sealed record PermissionQuery : IQuery<PermissionResponse>
{
    public Guid Permission { get; init; } = default!;
}

public sealed record PermissionResponse()
{
    public Guid Id { get; set; }

    public Name Name { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}

public sealed class PermissionQueryValidator : AbstractValidator<PermissionQuery>
{
    public PermissionQueryValidator()
    {
        RuleFor(request => request.Permission)
            .NotEmpty().WithMessage("Enter Permission")
            .NotNull().WithMessage("Enter Permission");
    }
}