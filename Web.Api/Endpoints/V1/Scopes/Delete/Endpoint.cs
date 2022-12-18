using Application.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes.Delete;

internal sealed class Endpoint : Endpoint<DeleteScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}");
        Permissions("scopes_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a scope in the system";
        Description = "Delete a scope in the system";
        Response(204, "Scope was successfully deleted");
    }
}

internal sealed class RequestValidator : Validator<DeleteScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");
    }
}