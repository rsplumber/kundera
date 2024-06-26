using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Roles.Meta;

file sealed class Endpoint : Endpoint<ChangeRoleMetaCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles/{roleId:guid}/meta");
        Permissions("roles_change_meta");
        Version(1);
    }

    public override async Task HandleAsync(ChangeRoleMetaCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change role meta in the system";
        Description = "Change role meta in the system";
        Response(200, "Role meta was successfully changed");
    }
}

file sealed class RequestValidator : Validator<ChangeRoleMetaCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter a RoleId")
            .NotNull().WithMessage("Enter a RoleId");
    }
}