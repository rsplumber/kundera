using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Services.Add;

file sealed class Endpoint : Endpoint<AddScopeServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/services");
        Permissions("scopes_add_service");
        Version(1);
    }

    public override async Task HandleAsync(AddScopeServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add scope service in the system";
        Description = "Add scope service in the system";
        Response(200, "Scope service was successfully added");
    }
}

file sealed class RequestValidator : Validator<AddScopeServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");

        RuleFor(request => request.Services)
            .NotEmpty().WithMessage("Enter at least one service")
            .NotNull().WithMessage("Enter at least one service");
    }
}