using Data.Abstractions;
using Data.Abstractions.Users;
using FastEndpoints;
using Mediator;

namespace Application.Users.AuthenticateActivities.Internal;

file sealed class Endpoint : Endpoint<UserAuthenticateActivitiesQuery, PageableResponse<UserAuthenticateActivitiesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("internal/users/{userId}/authenticate_activities");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserAuthenticateActivitiesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User authenticate activities list";
        Description = "User authenticate activities list";
        Response<PageableResponse<UserAuthenticateActivitiesResponse>>(200, "Users was successfully received");
    }
}