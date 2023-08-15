using Data.Abstractions.Permissions;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Permissions.Details;

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
        Permissions("permissions_details");
        Version(1);
    }

    public override async Task HandleAsync(PermissionQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission details";
        Description = "Permission details";
        Response<PermissionResponse>(200, "Successful");
    }
}

file sealed class RequestValidator : Validator<PermissionQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.PermissionId)
            .NotEmpty().WithMessage("Enter a PermissionId")
            .NotNull().WithMessage("Enter a PermissionId");
    }
}