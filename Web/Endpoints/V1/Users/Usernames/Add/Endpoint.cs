using Application.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Users.Usernames.Add;

internal sealed class Endpoint : Endpoint<AddUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/usernames");
        Permissions("users_add_username");
        Version(1);
    }

    public override async Task HandleAsync(AddUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add user username in the system";
        Description = "Add user username in the system";
        Response(200, "User username was successfully added");
    }
}

internal sealed class RequestValidator : Validator<AddUserUsernameCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter a UserId")
            .NotNull().WithMessage("Enter a UserId");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter a Username")
            .NotNull().WithMessage("Enter a Username")
            .MinimumLength(4).WithMessage("Username must have at least 4 chars");
    }
}