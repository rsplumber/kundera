using Application.UserGroups;
using Domain.UserGroups;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.UserGroups;

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