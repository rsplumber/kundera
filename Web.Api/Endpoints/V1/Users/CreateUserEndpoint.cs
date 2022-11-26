using FastEndpoints;
using Managements.Application.Users;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class CreateUserEndpoint : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<UserEndpoint>(new {user.Id}, new UserResponse
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

internal sealed class CreateUserEndpointSummary : Summary<CreateUserEndpoint>
{
    public CreateUserEndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}