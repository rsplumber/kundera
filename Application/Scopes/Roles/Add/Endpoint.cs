using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Roles.Add;

file sealed class Endpoint : Endpoint<AddScopeRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/roles");
        Permissions("scopes_add_role");
        Version(1);
    }

    public override async Task HandleAsync(AddScopeRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add scope role in the system";
        Description = "Add scope role in the system";
        Response(200, "Scope role was successfully added");
    }
}

file sealed class RequestValidator : Validator<AddScopeRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");

        RuleFor(request => request.RolesIds)
            .NotEmpty().WithMessage("Enter at least one role")
            .NotNull().WithMessage("Enter at least one role");
    }
}