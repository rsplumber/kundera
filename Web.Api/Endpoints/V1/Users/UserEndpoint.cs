using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Users;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class UserEndpoint : Endpoint<UserQuery, UserResponse>
{
    private readonly IMediator _mediator;

    public UserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class UserEndpointSummary : Summary<UserEndpoint>
{
    public UserEndpointSummary()
    {
        Summary = "User details";
        Description = "User details";
        Response<GroupResponse>(200, "User was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}