using FastEndpoints;
using Mediator;
using Queries;
using Queries.Groups;
using Queries.Users;

namespace Application.Users.List;

internal sealed class Endpoint : Endpoint<UsersQuery, PageableResponse<UsersResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users");
        Permissions("users_list");
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
        Response<PageableResponse<GroupResponse>>(200, "Users was successfully received");
    }
}