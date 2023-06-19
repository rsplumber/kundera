using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Services.Delete;

internal sealed class Endpoint : Endpoint<DeleteScopeServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}/services");
        Permissions("scopes_remove_service");
        Version(1);
    }

    public override async Task HandleAsync(DeleteScopeServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove scope service in the system";
        Description = "Remove scope service in the system";
        Response(204, "Scope service was successfully removed");
    }
}

internal sealed class RequestValidator : Validator<DeleteScopeServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");

        RuleFor(request => request.ServicesIds)
            .NotEmpty().WithMessage("Enter at least one service")
            .NotNull().WithMessage("Enter at least one service");
    }
}