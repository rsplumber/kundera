using FastEndpoints;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class CreateScopeEndpoint : Endpoint<CreateScopeCommand>
{
    private readonly IMediator _mediator;

    public CreateScopeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateScopeCommand req, CancellationToken ct)
    {
        var scope = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<ScopeEndpoint>(new {scope.Id}, new ScopeResponse
            {
                Id = scope.Id.Value,
                Name = scope.Name,
                Secret = scope.Secret,
                Status = scope.Status.ToString()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class CreateScopeEndpointSummary : Summary<CreateScopeEndpoint>
{
    public CreateScopeEndpointSummary()
    {
        Summary = "Create a new role in the system";
        Description = "Create a new role in the system";
        Response(201, "Scope was successfully created");
    }
}