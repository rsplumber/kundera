using FastEndpoints;
using Mediator;

namespace Application.Users.Usernames.Add.Internal;

file sealed class Endpoint : Endpoint<AddUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/{userId:guid}/usernames");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AddUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add user username in the system";
        Description = "Add user username in the system";
        Response(200, "User username was successfully added");
    }
}