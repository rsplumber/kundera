﻿using FluentValidation;
using RoleManagement.Application.Scopes;
using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Scopes;

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