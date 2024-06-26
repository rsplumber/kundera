using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Groups.Join;

file sealed class Endpoint : Endpoint<JoinUserToGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/groups");
        Permissions("users_join_group");
        Version(1);
    }

    public override async Task HandleAsync(JoinUserToGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Joint a user to a group role in the system";
        Description = "Joint a user to a group in the system";
        Response(200, "User was successfully joined to the group");
    }
}

file sealed class RequestValidator : Validator<JoinUserToGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter UserId")
            .NotNull().WithMessage("Enter UserId");

        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}