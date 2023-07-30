using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Status.Activate;

file sealed class Endpoint : Endpoint<ActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/active");
        Permissions("scopes_activate");
        Version(1);
    }

    public override async Task HandleAsync(ActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Activate a scope in the system";
        Description = "Activate a scope in the system";
        Response(200, "Scopes was successfully Activated");
    }
}

file sealed class RequestValidator : Validator<ActivateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter ScopeId")
            .NotNull().WithMessage("Enter ScopeId");
    }
}