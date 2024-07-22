using System.Net;
using Data.Abstractions.Users;
using FastEndpoints;
using Mediator;

namespace Application.Users.Create.Internal;

file sealed class Endpoint : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);
        await SendAsync(new UserResponse
        {
            Id = user.Id,
            Status = user.Status.ToString(),
            Groups = user.Groups.Select(g => g.Id).ToList(),
            Roles = user.Roles.Select(r => r.Id).ToList()
        }, (int)HttpStatusCode.Created, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}