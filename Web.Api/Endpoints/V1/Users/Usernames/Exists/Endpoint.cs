using Application.Groups;
using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users.Usernames.Exists;

internal sealed class Endpoint : Endpoint<ExistUserUsernameQuery, Guid>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{username}/exists");
        Permissions("user_exist_username");
        Version(1);
    }

    public override async Task HandleAsync(ExistUserUsernameQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "UserExist check";
        Description = "UserExist check";
        Response<GroupResponse>(200, "UserExist was successfully checked");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}