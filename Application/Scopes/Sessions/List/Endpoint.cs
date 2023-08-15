using Data.Abstractions.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Sessions.List;

file sealed class Endpoint : Endpoint<ScopeSessionsQuery, List<ScopeSessionResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{scopeId:guid}/sessions");
        Permissions("scopes_sessions_list");
        Version(1);
    }

    public override async Task HandleAsync(ScopeSessionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope sessions";
        Description = "List of scope active sessions";
        Response(200, "Sessions received successfully");
    }
}

file sealed class RequestValidator : Validator<ScopeSessionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter valid ScopeId")
            .NotNull().WithMessage("Enter valid ScopeId");
    }
}