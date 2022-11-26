using Core.Domains;
using FluentValidation;
using Mediator;

namespace Application.Permissions;

public sealed record PermissionQuery : IQuery<PermissionResponse>
{
    public Guid Permission { get; init; } = default!;
}

public sealed record PermissionResponse(Guid Id, Name Name)
{
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