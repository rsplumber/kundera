using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Status.Activate;

file sealed class Endpoint : Endpoint<ActiveUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/activate");
        Permissions("users_activate");
        Version(1);
    }

    public override async Task HandleAsync(ActiveUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Activate a user in the system";
        Description = "Activate a user in the system";
        Response(200, "Users was successfully Activated");
    }
}

file sealed class RequestValidator : Validator<ActiveUserCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");
    }
}