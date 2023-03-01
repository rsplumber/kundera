using Application.Groups;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Groups.Delete;

internal sealed class Endpoint : Endpoint<DeleteGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{groupId:guid}");
        Permissions("kundera_groups_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a  group in the system";
        Description = "Delete a group in the system";
        Response(204, "Group was successfully deleted");
    }
}

internal sealed class RequestValidator : Validator<DeleteGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");
    }
}