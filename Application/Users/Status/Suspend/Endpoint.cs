using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Status.Suspend;

file sealed class Endpoint : Endpoint<SuspendUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/suspend");
        Permissions("users_suspend");
        Version(1);
    }

    public override async Task HandleAsync(SuspendUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Suspend a user in the system";
        Description = "Suspend a user in the system";
        Response(200, "Users was successfully Suspended");
    }
}

file sealed class RequestValidator : Validator<SuspendUserCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");
    }
}