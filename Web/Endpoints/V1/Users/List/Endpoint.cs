using FastEndpoints;
using Mediator;
using Queries.Groups;
using Queries.Users;

namespace Web.Endpoints.V1.Users.List;

internal sealed class Endpoint : Endpoint<UsersQuery, List<UsersResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users");
        Permissions("kundera_users_list");
        Version(1);
    }

    public override async Task HandleAsync(UsersQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User list";
        Description = "User list";
        Response<GroupResponse>(200, "Users was successfully received");
    }
}