using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Status.DeActivate;

file sealed class Endpoint : Endpoint<DeActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/de-active");
        Permissions("scopes_de-activate");
        Version(1);
    }

    public override async Task HandleAsync(DeActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "DeActivate a scope in the system";
        Description = "DeActivate a scope in the system";
        Response(200, "Scopes was successfully DeActivated");
    }
}

file sealed class RequestValidator : Validator<DeActivateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");
    }
}