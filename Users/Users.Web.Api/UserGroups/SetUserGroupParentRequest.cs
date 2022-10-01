using FluentValidation;
using Tes.Web.Validators;
using Users.Application.UserGroups;
using Users.Domain.UserGroups;

namespace Users.Web.Api.UserGroups;

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