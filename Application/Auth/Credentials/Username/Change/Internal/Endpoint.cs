using Application.Auth.Credentials.Password.Change;
using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.Username.Change.Internal;

file sealed class Endpoint : Endpoint<CredentialChangeUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/credentials/username/change");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CredentialChangeUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change credential username";
        Description = "Change credential username";
        Response(200, "Successful");
    }
}