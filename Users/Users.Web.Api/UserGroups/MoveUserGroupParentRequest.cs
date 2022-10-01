using FluentValidation;
using Tes.Web.Validators;
using Users.Application.UserGroups;
using Users.Domain.UserGroups;

namespace Users.Web.Api.UserGroups;

public record MoveUserGroupParentRequest(Guid To) : IWebRequest
{
    public MoveUserGroupParentCommand ToCommand(Guid userGroupId) => new(UserGroupId.From(userGroupId), UserGroupId.From(To));
}

public class MoveUserGroupParentRequestValidator : RequestValidator<MoveUserGroupParentRequest>
{
    public MoveUserGroupParentRequestValidator()
    {
        
        RuleFor(request => request.To)
            .NotEmpty().WithMessage("Enter a valid UserGroupId")
            .NotNull().WithMessage("Enter a valid UserGroupId");
    }
}