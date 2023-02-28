using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Groups;
using Queries.Scopes;

namespace Web.Endpoints.V1.Scopes.Details;

internal sealed class Endpoint : Endpoint<ScopeQuery, ScopeResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{scopeId:guid}");
        Permissions("kundera_scopes_get");
        Version(1);
    }

    public override async Task HandleAsync(ScopeQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope details";
        Description = "Scope details";
        Response<GroupResponse>(200, "Scope was successfully received");
    }
}

internal sealed class RequestValidator : Validator<ScopeQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");
    }
}