using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.Delete;

file sealed class Endpoint : Endpoint<DeleteCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/credentials/{id}");
        Permissions("credentials_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove Credential";
        Description = "remove a Credential";
        Response(204, "Credential removed successfully");
    }
}

file sealed class RequestValidator : Validator<DeleteCredentialCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter valid id")
            .NotNull().WithMessage("Enter valid id");
    }
}