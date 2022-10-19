using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Scopes;
using Managements.Domain.Scopes;
using Managements.Domain.Services;

namespace Web.Api.Scopes;

public record AddScopeServiceRequest(List<Guid> ServiceIds) : IWebRequest
{
    public AddScopeServiceCommand ToCommand(Guid scopeId)
    {
        var services = ServiceIds.Select(ServiceId.From)
            .ToArray();

        return new(ScopeId.From(scopeId), services);
    }
}

public class AddScopeServiceRequestValidator : RequestValidator<AddScopeServiceRequest>
{
    public AddScopeServiceRequestValidator()
    {
        RuleFor(request => request.ServiceIds)
            .NotEmpty()
            .WithMessage("Enter valid Service")
            .NotNull()
            .WithMessage("Enter valid Service");
    }
}

public record RemoveScopeServiceRequest(List<Guid> ServiceIds) : IWebRequest
{
    public RemoveScopeServiceCommand ToCommand(Guid scopeId)
    {
        var services = ServiceIds.Select(ServiceId.From)
            .ToArray();

        return new(ScopeId.From(scopeId), services);
    }
}

public class RemoveScopeServiceRequestValidator : RequestValidator<RemoveScopeServiceRequest>
{
    public RemoveScopeServiceRequestValidator()
    {
        RuleFor(request => request.ServiceIds)
            .NotEmpty()
            .WithMessage("Enter valid Service")
            .NotNull()
            .WithMessage("Enter valid Service");
    }
}