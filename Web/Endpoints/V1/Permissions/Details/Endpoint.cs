using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Groups;
using Queries.Permissions;

namespace Web.Endpoints.V1.Permissions.Details;

internal sealed class Endpoint : Endpoint<PermissionQuery, PermissionResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("permissions/{permissionId:guid}");
        Permissions("permissions_get");
        Version(1);
    }

    public override async Task HandleAsync(PermissionQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission details";
        Description = "Permission details";
        Response<GroupResponse>(200, "Permission was successfully received");
    }
}

internal sealed class RequestValidator : Validator<PermissionQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.PermissionId)
            .NotEmpty().WithMessage("Enter PermissionId")
            .NotNull().WithMessage("Enter PermissionId");
    }
}