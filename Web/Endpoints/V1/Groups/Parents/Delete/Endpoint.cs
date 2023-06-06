using Application.Groups;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Groups.Parents.Delete;

internal sealed class Endpoint : Endpoint<RemoveGroupParentCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{groupId:guid}/parent");
        Permissions("groups_remove_parent");
        Version(1);
    }

    public override async Task HandleAsync(RemoveGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove a parent from a group in the system";
        Description = "Set a parent from a group in the system";
        Response(204, "Parent was successfully removed from the group");
    }
}

internal sealed class RequestValidator : Validator<RemoveGroupParentCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a GroupId")
            .NotNull().WithMessage("Enter a GroupId");
    }
}