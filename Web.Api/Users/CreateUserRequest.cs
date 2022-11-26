using Application.Users;
using Core.Domains.Groups.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Users;

public record CreateUserRequest(string Username, Guid Group) : IWebRequest
{
    public CreateUserCommand ToCommand() => new(Username, GroupId.From(Group));
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

        RuleFor(request => request.Group)
            .NotEmpty()
            .WithMessage("Enter a valid Group")
            .NotNull()
            .WithMessage("Enter a valid Group");
    }
}