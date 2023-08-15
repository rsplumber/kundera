using Data.Abstractions.Groups;
using Data.Abstractions.Services;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Services.Permissions.List;

file sealed class Endpoint : Endpoint<ServicePermissionsQuery, List<PermissionsResponse>>
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

    public override async Task HandleAsync(ServicePermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permissions list";
        Description = "Permissions list";
        Response<GroupResponse>(200, "Permissions was successfully received");
    }
}

file sealed class RequestValidator : Validator<ServicePermissionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");
    }
}