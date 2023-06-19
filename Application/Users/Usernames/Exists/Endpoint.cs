using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Users;

namespace Application.Users.Usernames.Exists;

internal sealed class Endpoint : Endpoint<ExistUserUsernameQuery, bool>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{username}/exists");
        Permissions("users_exist_username");
        Version(1);
    }

    public override async Task HandleAsync(ExistUserUsernameQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Check User exists";
        Description = "Check that a User with Username exists";
        Response<bool>(200, "Username checked");
    }
}

internal sealed class RequestValidator : Validator<ExistUserUsernameQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");
    }
}