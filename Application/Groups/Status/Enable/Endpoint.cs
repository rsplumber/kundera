using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Groups.Status.Enable;

file sealed class Endpoint : Endpoint<EnableGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/enable");
        Permissions("groups_enable");
        Version(1);
    }

    public override async Task HandleAsync(EnableGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Enable a group in the system";
        Description = "Enable a group in the system";
        Response(200, "Group was successfully Enabled");
    }
}

file sealed class RequestValidator : Validator<EnableGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a GroupId")
            .NotNull().WithMessage("Enter a GroupId");
    }
}