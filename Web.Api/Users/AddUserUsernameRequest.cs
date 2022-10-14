using Application.Users;
using Domain.Users;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Users;

public record AddUserUsernameRequest(string Username) : IWebRequest
{
    public AddUserUsernameCommand ToCommand(Guid userId) => new(UserId.From(userId), Domain.Users.Username.From(Username));
}

public class AddUserUsernameRequestValidator : RequestValidator<AddUserUsernameRequest>
{
    public AddUserUsernameRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(3)
            .WithMessage("Minimum username length : 3")
            .NotEmpty()
            .WithMessage("Enter a valid Username")
            .NotNull()
            .WithMessage("Enter a valid Username");
    }
}

public record RemoveUserUsernameRequest(string Username) : IWebRequest
{
    public RemoveUserUsernameCommand ToCommand(Guid userId) => new(UserId.From(userId), Domain.Users.Username.From(Username));
}

public class RemoveUserUsernameRequestValidator : RequestValidator<RemoveUserUsernameRequest>
{
    public RemoveUserUsernameRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(3)
            .WithMessage("Minimum username length : 3")
            .NotEmpty()
            .WithMessage("Enter a valid Username")
            .NotNull()
            .WithMessage("Enter a valid Username");
    }
}