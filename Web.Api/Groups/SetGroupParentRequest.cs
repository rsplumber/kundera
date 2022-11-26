using Application.Groups;
using Core.Domains.Groups.Types;
using FluentValidation;
using Kite.Web.Requests;

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