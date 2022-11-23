using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Groups;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Types;

namespace Web.Api.Groups;

public record SetGroupParentRequest(Guid ParentId) : IWebRequest
{
    public SetGroupParentCommand ToCommand(Guid groupId) => new(GroupId.From(groupId), GroupId.From(ParentId));
}

public class SetGroupParentRequestValidator : RequestValidator<SetGroupParentRequest>
{
    public SetGroupParentRequestValidator()
    {
        RuleFor(request => request.ParentId)
            .NotEmpty()
            .WithMessage("Enter a valid GroupId")
            .NotNull()
            .WithMessage("Enter a valid GroupId");
    }
}