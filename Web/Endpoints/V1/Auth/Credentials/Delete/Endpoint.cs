using Application.Auth.Credentials;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Auth.Credentials.Delete;

internal sealed class Endpoint : Endpoint<RemoveCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/credentials/{id}");
        Permissions("kundera_credentials_delete");
        Version(1);
    }

    public override async Task HandleAsync(RemoveCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove Credential";
        Description = "remove a Credential";
        Response(204, "Credential removed successfully");
    }
}

internal sealed class RequestValidator : Validator<RemoveCredentialCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter valid id")
            .NotNull().WithMessage("Enter valid id");
    }
}