using System.Net;
using Data.Abstractions.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Create;

file sealed class Endpoint : Endpoint<CreateScopeCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes");
        Permissions("scopes_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateScopeCommand req, CancellationToken ct)
    {
        var scope = await _mediator.Send(req, ct);
        await SendAsync(new ScopeResponse
        {
            Id = scope.Id,
            Name = scope.Name,
            Secret = scope.Secret,
            Status = scope.Status.ToString()
        }, (int)HttpStatusCode.Created, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new role in the system";
        Description = "Create a new role in the system";
        Response(201, "Scope was successfully created");
    }
}

file sealed class RequestValidator : Validator<CreateScopeCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");


        RuleFor(request => request.SessionTokenExpireTimeInMinutes)
            .NotEmpty().WithMessage("Enter valid SessionTokenExpireTimeInMinutes")
            .NotNull().WithMessage("Enter valid SessionTokenExpireTimeInMinutes")
            .GreaterThan(1).WithMessage("SessionTokenExpireTimeInMinutes must be greater than 1");


        RuleFor(request => request.SessionRefreshTokenExpireTimeInMinutes)
            .NotEmpty().WithMessage("Enter valid SessionRefreshTokenExpireTimeInMinutes")
            .NotNull().WithMessage("Enter valid SessionRefreshTokenExpireTimeInMinutes")
            .GreaterThan(1).WithMessage("SessionRefreshTokenExpireTimeInMinutes must be greater than 1");
    }
}