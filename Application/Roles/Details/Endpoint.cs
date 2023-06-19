using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Groups;
using Queries.Roles;

namespace Application.Roles.Details;

internal sealed class Endpoint : Endpoint<RoleQuery, RoleResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles/{roleId:guid}");
        Permissions("roles_get");
        Version(1);
    }

    public override async Task HandleAsync(RoleQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Role details";
        Description = "Role details";
        Response<GroupResponse>(200, "Role was successfully received");
    }
}

internal sealed class RequestValidator : Validator<RoleQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter RoleId")
            .NotNull().WithMessage("Enter RoleId");
    }
}