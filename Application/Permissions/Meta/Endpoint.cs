using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Permissions.Meta;

file sealed class Endpoint : Endpoint<ChangePermissionMetaCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("permissions/{permissionId:guid}/meta");
        AllowAnonymous();
        Permissions("permissions_change_meta");
        Version(1);
    }

    public override async Task HandleAsync(ChangePermissionMetaCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change permission meta in the system";
        Description = "Change permission meta in the system";
        Response(200, "Permission meta was successfully changed");
    }
}

file sealed class RequestValidator : Validator<ChangePermissionMetaCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.PermissionId)
            .NotEmpty().WithMessage("Enter a PermissionId")
            .NotNull().WithMessage("Enter a PermissionId");
    }
}