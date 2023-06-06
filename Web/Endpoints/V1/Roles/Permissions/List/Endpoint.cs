using Application.Groups;
using Application.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Roles.Permissions.List;

internal sealed class Endpoint : Endpoint<RolePermissionsQuery, List<RolePermissionsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles/{roleId:guid}/permissions");
        Permissions("roles_permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(RolePermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Role permissions list";
        Description = "Roles permissions list";
        Response<GroupResponse>(200, "Role permissions was successfully received");
    }
}

internal sealed class RequestValidator : Validator<RolePermissionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter RoleId")
            .NotNull().WithMessage("Enter RoleId");
    }
}