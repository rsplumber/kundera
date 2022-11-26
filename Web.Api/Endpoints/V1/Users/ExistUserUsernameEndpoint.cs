using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Users;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class ExistUserUsernameEndpoint : Endpoint<ExistUserUsernameQuery, Guid>
{
    private readonly IMediator _mediator;

    public ExistUserUsernameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ExistUserUsernameQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class ExistUserUsernameEndpointSummary : Summary<ExistUserUsernameEndpoint>
{
    public ExistUserUsernameEndpointSummary()
    {
        Summary = "UserExist check";
        Description = "UserExist check";
        Response<GroupResponse>(200, "UserExist was successfully checked");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}