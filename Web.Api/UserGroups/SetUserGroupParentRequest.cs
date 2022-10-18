using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.UserGroups;
using Managements.Domain.UserGroups;

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
            .NotEmpty()
            .WithMessage("Enter a valid UserGroupId")
            .NotNull()
            .WithMessage("Enter a valid UserGroupId");
    }
}