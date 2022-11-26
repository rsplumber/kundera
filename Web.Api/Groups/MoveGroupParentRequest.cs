using Application.Groups;
using Core.Domains.Groups.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Groups;

public record MoveGroupParentRequest(Guid To) : IWebRequest
{
    public MoveGroupParentCommand ToCommand(Guid id) => new(GroupId.From(id), GroupId.From(To));
}

public class MoveGroupParentRequestValidator : RequestValidator<MoveGroupParentRequest>
{
    public MoveGroupParentRequestValidator()
    {
        RuleFor(request => request.To)
            .NotEmpty()
            .WithMessage("Enter a valid GroupId")
            .NotNull()
            .WithMessage("Enter a valid GroupId");
    }
}