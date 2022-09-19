using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Web.Api.Users;

public record JoinUserToGroupRequest(Guid UserGroup) : IWebRequest
{
    public JoinUserToGroupCommand ToCommand(Guid userId) => new(UserId.From(userId), UserGroupId.From(UserGroup));
}

public class JoinUserToGroupRequestValidator : RequestValidator<JoinUserToGroupRequest>
{
    public JoinUserToGroupRequestValidator()
    {
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}

public record RemoveUserFromGroupRequest(Guid UserGroup) : IWebRequest
{
    public RemoveUserFromGroupCommand ToCommand(Guid userId) => new(UserId.From(userId), UserGroupId.From(UserGroup));
}

public class RemoveUserFromGroupRequestValidator : RequestValidator<RemoveUserFromGroupRequest>
{
    public RemoveUserFromGroupRequestValidator()
    {
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}