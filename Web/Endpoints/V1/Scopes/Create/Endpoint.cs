using Application.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Scopes.Create;

internal sealed class Endpoint : Endpoint<CreateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes");
        Permissions("kundera_scopes_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateScopeCommand req, CancellationToken ct)
    {
        var scope = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new { scope.Id }, new ScopeResponse
            {
                Id = scope.Id,
                Name = scope.Name,
                Secret = scope.Secret,
                Status = scope.Status.ToString()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new role in the system";
        Description = "Create a new role in the system";
        Response(201, "Scope was successfully created");
    }
}

internal sealed class RequestValidator : Validator<CreateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}