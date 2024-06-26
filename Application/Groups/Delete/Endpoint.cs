using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Groups.Delete;

file sealed class Endpoint : Endpoint<DeleteGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{groupId:guid}");
        Permissions("groups_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a  group in the system";
        Description = "Delete a group in the system";
        Response(204, "Group was successfully deleted");
    }
}

file sealed class RequestValidator : Validator<DeleteGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");
    }
}