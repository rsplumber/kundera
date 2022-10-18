using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Users;
using Managements.Domain.UserGroups;

namespace Web.Api.Users;

public record CreateUserRequest(string Username, Guid UserGroup) : IWebRequest
{
    public CreateUserCommand ToCommand() => new(Username, UserGroupId.From(UserGroup));
}

public class CreateUserRequestValidator : RequestValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(2)
            .WithMessage("Username minimum length is 2")
            .MaximumLength(30)
            .WithMessage("Username Maximum length is 30")
            .NotEmpty()
            .WithMessage("Enter a valid Username")
            .NotNull()
            .WithMessage("Enter a valid Username");

        RuleFor(request => request.UserGroup)
            .NotEmpty()
            .WithMessage("Enter a valid UserGroup")
            .NotNull()
            .WithMessage("Enter a valid UserGroup");
    }
}