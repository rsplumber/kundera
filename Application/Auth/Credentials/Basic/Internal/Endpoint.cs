using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.Basic.Internal;

file sealed class Endpoint : Endpoint<CreateBasicCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/credentials");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateBasicCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create Credential";
        Description = "Create a default Credential";
        Response(200, "Credential Created successfully");
    }
}