using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.Sessions.Terminate.Internal;

file sealed class Endpoint : Endpoint<TerminateCredentialSessionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("internal/credentials/{id}/session/terminate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(TerminateCredentialSessionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove Credential";
        Description = "remove a Credential";
        Response(204, "Credential removed successfully");
    }
}