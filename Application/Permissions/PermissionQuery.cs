using Core.Domains;
using FluentValidation;
using Mediator;

namespace Application.Permissions;

public sealed record PermissionQuery : IQuery<PermissionResponse>
{
    public Guid Permission { get; init; } = default!;
}

public sealed record PermissionResponse
{
    public Guid Id { get; init; }

    public Name Name { get; init; } = default!;

    public Dictionary<string, string>? Meta { get; init; }
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