using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Users;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Types;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Web.Api.Users;

public record JoinUserToGroupRequest(Guid Group) : IWebRequest
{
    public JoinUserToGroupCommand ToCommand(Guid userId) => new(UserId.From(userId), GroupId.From(Group));
}

public class JoinUserToGroupRequestValidator : RequestValidator<JoinUserToGroupRequest>
{
    public JoinUserToGroupRequestValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty()
            .WithMessage("Enter a valid Group")
            .NotNull()
            .WithMessage("Enter a valid Group");
    }
}

public record RemoveUserFromGroupRequest(Guid Group) : IWebRequest
{
    public RemoveUserFromGroupCommand ToCommand(Guid userId) => new(UserId.From(userId), GroupId.From(Group));
}

public class RemoveUserFromGroupRequestValidator : RequestValidator<RemoveUserFromGroupRequest>
{
    public RemoveUserFromGroupRequestValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty()
            .WithMessage("Enter a valid Group")
            .NotNull()
            .WithMessage("Enter a valid Group");
    }
}