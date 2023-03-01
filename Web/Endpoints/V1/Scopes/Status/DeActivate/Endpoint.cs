using Application.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Scopes.Status.DeActivate;

internal sealed class Endpoint : Endpoint<DeActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/de-active");
        Permissions("kundera_scopes_de-activate");
        Version(1);
    }

    public override async Task HandleAsync(DeActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "DeActivate a scope in the system";
        Description = "DeActivate a scope in the system";
        Response(200, "Scopes was successfully DeActivated");
    }
}

internal sealed class RequestValidator : Validator<DeActivateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");
    }
}