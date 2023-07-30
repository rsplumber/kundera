using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Roles.Delete;

file sealed class Endpoint : Endpoint<DeleteScopeRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}/roles");
        Permissions("scopes_remove_role");
        Version(1);
    }

    public override async Task HandleAsync(DeleteScopeRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove scope role in the system";
        Description = "Remove scope role in the system";
        Response(204, "Scope role was successfully removed");
    }
}

file sealed class RequestValidator : Validator<DeleteScopeRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.RolesIds)
            .NotEmpty().WithMessage("Enter at least one role")
            .NotNull().WithMessage("Enter at least one role");
    }
}