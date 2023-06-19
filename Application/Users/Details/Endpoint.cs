using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Groups;
using Queries.Users;

namespace Application.Users.Details;

internal sealed class Endpoint : Endpoint<UserQuery, UserResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{userId:guid}");
        Permissions("users_get");
        Version(1);
    }

    public override async Task HandleAsync(UserQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User details";
        Description = "User details";
        Response<GroupResponse>(200, "User was successfully received");
    }
}

internal sealed class RequestValidator : Validator<UserQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");
    }
}