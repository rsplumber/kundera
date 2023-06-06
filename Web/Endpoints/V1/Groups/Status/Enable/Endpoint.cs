using Application.Groups;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Groups.Status.Enable;

internal sealed class Endpoint : Endpoint<EnableGroupCommand>
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

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Enable a group in the system";
        Description = "Enable a group in the system";
        Response(200, "Group was successfully Enabled");
    }
}

internal sealed class RequestValidator : Validator<EnableGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a GroupId")
            .NotNull().WithMessage("Enter a GroupId");
    }
}