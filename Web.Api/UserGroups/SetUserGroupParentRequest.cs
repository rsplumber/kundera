﻿using Application.UserGroups;
using Domain.UserGroups;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.UserGroups;

public record SetUserGroupParentRequest(Guid ParentId) : IWebRequest
{
    public SetUserGroupParentCommand ToCommand(Guid userGroupId) => new(UserGroupId.From(userGroupId), UserGroupId.From(ParentId));
}

public class SetUserGroupParentRequestValidator : RequestValidator<SetUserGroupParentRequest>
{
    public SetUserGroupParentRequestValidator()
    {
        RuleFor(request => request.ParentId)
            .NotEmpty().WithMessage("Enter a valid UserGroupId")
            .NotNull().WithMessage("Enter a valid UserGroupId");
    }
}