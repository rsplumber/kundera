using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Groups.Parents.Set;

file sealed class Endpoint : Endpoint<SetGroupParentCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/parent");
        Permissions("groups_set_parent");
        Version(1);
    }

    public override async Task HandleAsync(SetGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Set a parent to a group in the system";
        Description = "Set a parent to a group in the system";
        Response(200, "Parent was successfully set to the group");
    }
}

file sealed class RequestValidator : Validator<SetGroupParentCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a GroupId")
            .NotNull().WithMessage("Enter a GroupId");

        RuleFor(request => request.ParentId)
            .NotEmpty().WithMessage("Enter a Parent")
            .NotNull().WithMessage("Enter a Parent");
    }
}