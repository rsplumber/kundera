using Application.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Users.Groups.Join;

internal sealed class Endpoint : Endpoint<JoinUserToGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/groups");
        Permissions("user_join_group");
        Version(1);
    }

    public override async Task HandleAsync(JoinUserToGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Joint a user to a group role in the system";
        Description = "Joint a user to a group in the system";
        Response(200, "User was successfully joined to the group");
    }
}

internal sealed class RequestValidator : Validator<JoinUserToGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}