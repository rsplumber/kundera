using Data.Abstractions.Users;
using FastEndpoints;
using Mediator;

namespace Application.Users.Usernames.Credential.Internal;

file sealed class Endpoint : Endpoint<UserUsernameCredentialQuery, UserUsernameCredentialQueryResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("internal/users/usernames/{username}/credential");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserUsernameCredentialQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Get username credential";
        Description = "Get username credential";
        Response<UserUsernameCredentialQueryResponse>(200, "Successful");
    }
}