using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.OneTime.Internal;

file sealed class Endpoint : Endpoint<CreateOneTimeCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/credentials/one-time");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateOneTimeCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create OneTime Credential";
        Description = "Create a OneTime Credential that expires after one use";
        Response(200, "Credential Created successfully");
    }
}