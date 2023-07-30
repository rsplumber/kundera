using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Status.Block;

file sealed class Endpoint : Endpoint<BlockUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/block");
        Permissions("users_block");
        Version(1);
    }

    public override async Task HandleAsync(BlockUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Block a user in the system";
        Description = "Block a user in the system";
        Response(200, "Users was successfully Blocked");
    }
}

file sealed class RequestValidator : Validator<BlockUserCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");

        RuleFor(request => request.Reason)
            .NotEmpty().WithMessage("Enter Reason")
            .NotNull().WithMessage("Enter Reason");
    }
}