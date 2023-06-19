using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Permissions.Delete;

internal sealed class Endpoint : Endpoint<DeletePermissionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("permissions/{permissionId:guid}");
        Permissions("permissions_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeletePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a permission in the system";
        Description = "Delete a permission in the system";
        Response(204, "Permission was successfully deleted");
    }
}

internal sealed class RequestValidator : Validator<DeletePermissionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.PermissionId)
            .NotEmpty().WithMessage("Enter a PermissionId")
            .NotNull().WithMessage("Enter a PermissionId");
    }
}