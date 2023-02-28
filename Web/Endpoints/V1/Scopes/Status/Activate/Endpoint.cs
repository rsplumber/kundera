using Commands.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Scopes.Status.Activate;

internal sealed class Endpoint : Endpoint<ActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/active");
        Permissions("kundera_scopes_activate");
        Version(1);
    }

    public override async Task HandleAsync(ActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ActivateScopeEndpointSummary : Summary<Endpoint>
{
    public ActivateScopeEndpointSummary()
    {
        Summary = "Activate a scope in the system";
        Description = "Activate a scope in the system";
        Response(200, "Scopes was successfully Activated");
    }
}

internal sealed class RequestValidator : Validator<ActivateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter ScopeId")
            .NotNull().WithMessage("Enter ScopeId");
    }
}