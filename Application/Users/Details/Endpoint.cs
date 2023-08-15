using Data.Abstractions.Groups;
using Data.Abstractions.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Details;

file sealed class Endpoint : Endpoint<UserQuery, UserResponse>
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

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User details";
        Description = "User details";
        Response<GroupResponse>(200, "User was successfully received");
    }
}

file sealed class RequestValidator : Validator<UserQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");
    }
}