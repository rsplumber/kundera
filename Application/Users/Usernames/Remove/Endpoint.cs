using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Usernames.Remove;

file sealed class Endpoint : Endpoint<RemoveUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}/usernames");
        Permissions("users_remove_username");
        Version(1);
    }

    public override async Task HandleAsync(RemoveUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove user username in the system";
        Description = "Remove user username in the system";
        Response(204, "User username was successfully removed");
    }
}

file sealed class RequestValidator : Validator<RemoveUserUsernameCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");
    }
}