using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Scopes;

namespace Web.Endpoints.V1.Scopes.Sessions.List;

internal sealed class Endpoint : Endpoint<ScopeSessionsQuery, List<ScopeSessionResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}/sessions");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ScopeSessionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope sessions";
        Description = "List of scope active sessions";
        Response(200, "Sessions received successfully");
    }
}

internal sealed class RequestValidator : Validator<ScopeSessionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter valid ScopeId")
            .NotNull().WithMessage("Enter valid ScopeId");
    }
}