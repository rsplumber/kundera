using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Users;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class UsersEndpoint : Endpoint<UsersQuery, List<UsersResponse>>
{
    private readonly IMediator _mediator;

    public UsersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UsersQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class UsersEndpointSummary : Summary<UsersEndpoint>
{
    public UsersEndpointSummary()
    {
        Summary = "User list";
        Description = "User list";
        Response<GroupResponse>(200, "Users was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}