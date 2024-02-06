using FastEndpoints;
using Mediator;

namespace Application.Auth.Credentials.Password.Change.Internal;

file sealed class Endpoint : Endpoint<CredentialChangePasswordCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/credentials/password/change");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CredentialChangePasswordCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change credential password";
        Description = "Change credential password";
        Response(200, "Successful");
    }
}