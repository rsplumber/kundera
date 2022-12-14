using Application.Users;
using FastEndpoints;
using Mediator;
using Web.Api.Endpoints.V1.Users.Details;

namespace Web.Api.Endpoints.V1.Users.Create;

internal sealed class Endpoint : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users");
        Permissions("user_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new {user.Id}, new UserResponse
            {
                Id = user.Id.Value,
                Usernames = user.Usernames.Select(username => username.Value).ToList(),
                Status = user.Status.ToString(),
                Groups = user.Groups.Select(id => id.Value).ToList(),
                Roles = user.Roles.Select(id => id.Value).ToList()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}