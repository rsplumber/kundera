using Data.Abstractions.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Usernames.Credential;

file sealed class Endpoint : Endpoint<UserUsernameCredentialQuery, UserUsernameCredentialQueryResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/usernames/{username}/credential");
        Permissions("users_username_credential");
        Version(1);
    }

    public override async Task HandleAsync(UserUsernameCredentialQuery req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Get username credential";
        Description = "Get username credential";
        Response<UserUsernameCredentialQueryResponse>(200, "Successful");
    }
}

file sealed class RequestValidator : Validator<UserUsernameCredentialQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");
    }
}