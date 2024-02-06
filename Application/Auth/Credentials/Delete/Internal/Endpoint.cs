using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.Delete.Internal;

file sealed class Endpoint : Endpoint<DeleteCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("internal/users/credentials/{id}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeleteCredentialCommand req, CancellationToken ct)
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