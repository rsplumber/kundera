using Application.Groups;
using Core.Domains.Groups.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Groups;

public record CreateGroupRequest(string Name, Guid RoleId, Guid? ParentId = null) : IWebRequest
{
    public CreateGroupCommand ToCommand() => new(Name, Core.Domains.Roles.Types.RoleId.From(RoleId))
    {
        Parent = ParentId is not null ? GroupId.From(ParentId.Value) : null
    };
}

public class CreateGroupRequestValidator : RequestValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(6)
            .WithMessage("Group name minimum length 3")
            .NotEmpty()
            .WithMessage("Enter a valid Group name")
            .NotNull()
            .WithMessage("Enter a valid Group name");

        RuleFor(request => request.RoleId)
            .NotEmpty()
            .WithMessage("Enter a valid RoleId")
            .NotNull()
            .WithMessage("Enter a valid RoleId");
    }
}