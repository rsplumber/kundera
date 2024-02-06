using Data.Abstractions.Users;
using FastEndpoints;
using Mediator;

namespace Application.Users.Usernames.Exists.Internal;

file sealed class Endpoint : Endpoint<UserUsernameExistsQuery, UserUsernameExistsResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("internal/users/{username}/exists");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserUsernameExistsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Check User exists";
        Description = "Check that a User with Username exists";
        Response<UserUsernameExistsResponse>(200, "Username checked");
    }
}