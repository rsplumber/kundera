using Application.Groups;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups.Parents.Move;

internal sealed class Endpoint : Endpoint<MoveGroupParentCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/parent/move");
        Permissions("groups_move_parent");
        Version(1);
    }

    public override async Task HandleAsync(MoveGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Move group parent in the system";
        Description = "Move group parent in the system";
        Response(200, "Parent was successfully moved");
    }
}

internal sealed class RequestValidator : Validator<MoveGroupParentCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.To)
            .NotEmpty().WithMessage("Enter a To")
            .NotNull().WithMessage("Enter a To");
    }
}