using Application.Auth.Credentials;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class DeleteCredentialEndpoint : Endpoint<RemoveCredentialCommand>
{
    private readonly IMediator _mediator;

    public DeleteCredentialEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("credentials/{uniqueIdentifier}");
        Permissions("credentials_delete");
        Version(1);
    }

    public override async Task HandleAsync(RemoveCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteCredentialEndpointSummary : Summary<DeleteCredentialEndpoint>
{
    public DeleteCredentialEndpointSummary()
    {
        Summary = "Remove Credential";
        Description = "remove a Credential";
        Response(204, "Credential removed successfully");
    }
}