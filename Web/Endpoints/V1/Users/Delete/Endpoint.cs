using Commands.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Users.Delete;

internal sealed class Endpoint : Endpoint<DeleteUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}");
        Permissions("kundera_users_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a user in the system";
        Description = "Delete a user in the system";
        Response(204, "User was successfully deleted");
    }
}

internal sealed class RequestValidator : Validator<DeleteUserCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter a UserId")
            .NotNull().WithMessage("Enter a UserId");
    }
}