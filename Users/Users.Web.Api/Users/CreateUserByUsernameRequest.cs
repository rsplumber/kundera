﻿using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

public record CreateUserByUsernameRequest(string Username, Guid UserGroup) : IWebRequest
{

    public CreateUserByUsernameCommand ToCommand() => new(Username, UserGroupId.From(UserGroup));
}

public class CreateUserByUsernameRequestValidator : RequestValidator<CreateUserByUsernameRequest>
{
    public CreateUserByUsernameRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(2).WithMessage("Username minimum length is 2")
            .MaximumLength(30).WithMessage("Username Maximum length is 30")
            .NotEmpty().WithMessage("Enter a valid Username")
            .NotNull().WithMessage("Enter a valid Username");
        
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}