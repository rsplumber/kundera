using Commands.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Users.Usernames.Remove;

internal sealed class Endpoint : Endpoint<RemoveUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}/usernames");
        Permissions("kundera_users_remove_username");
        Version(1);
    }

    public override async Task HandleAsync(RemoveUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSSummary : Summary<Endpoint>
{
    public EndpointSSummary()
    {
        Summary = "Remove user username in the system";
        Description = "Remove user username in the system";
        Response(204, "User username was successfully removed");
    }
}

internal sealed class RequestValidator : Validator<RemoveUserUsernameCommand>
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