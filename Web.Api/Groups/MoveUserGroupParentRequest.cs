using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Groups;
using Managements.Domain.Groups;

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