﻿using Application.Scopes;
using Domain.Scopes;
using Domain.Services;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.Scopes;

public record AddScopeServiceRequest(List<string> ServiceIds) : IWebRequest
{
    public AddScopeServiceCommand ToCommand(string scopeId)
    {
        var services = ServiceIds.Select(ServiceId.From).ToArray();
        return new(ScopeId.From(scopeId), services);
    }
}

public class AddScopeServiceRequestValidator : RequestValidator<AddScopeServiceRequest>
{
    public AddScopeServiceRequestValidator()
    {
        RuleFor(request => request.ServiceIds)
            .NotEmpty().WithMessage("Enter valid Service")
            .NotNull().WithMessage("Enter valid Service");
    }
}

public record RemoveScopeServiceRequest(List<string> ServiceIds) : IWebRequest
{
    public RemoveScopeServiceCommand ToCommand(string scopeId)
    {
        var services = ServiceIds.Select(ServiceId.From).ToArray();
        return new(ScopeId.From(scopeId), services);
    }
}

public class RemoveScopeServiceRequestValidator : RequestValidator<RemoveScopeServiceRequest>
{
    public RemoveScopeServiceRequestValidator()
    {
        RuleFor(request => request.ServiceIds)
            .NotEmpty().WithMessage("Enter valid Service")
            .NotNull().WithMessage("Enter valid Service");
    }
}