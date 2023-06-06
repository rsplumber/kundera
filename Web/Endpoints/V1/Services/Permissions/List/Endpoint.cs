using Application.Groups;
using Application.Permissions;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Services.Permissions.List;

internal sealed class Endpoint : Endpoint<PermissionsQuery, List<PermissionsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("services/{serviceId}/permissions");
        Permissions("permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(PermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permissions list";
        Description = "Permissions list";
        Response<GroupResponse>(200, "Permissions was successfully received");
    }
}

internal sealed class RequestValidator : Validator<PermissionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");
    }
}