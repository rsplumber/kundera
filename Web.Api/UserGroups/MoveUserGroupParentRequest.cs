using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.UserGroups;
using Managements.Domain.UserGroups;

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
            .NotEmpty()
            .WithMessage("Enter a valid UserGroupId")
            .NotNull()
            .WithMessage("Enter a valid UserGroupId");
    }
}