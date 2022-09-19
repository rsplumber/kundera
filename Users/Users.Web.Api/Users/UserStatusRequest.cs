using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Web.Api.Users;

public record ActiveUserStatusRequest(Guid User, string? Reason) : IWebRequest
{
    public ActiveUserCommand ToCommand() => new(UserId.From(User), Text.From(Reason!));
}

public class ActiveUserStatusRequestValidator : RequestValidator<ActiveUserStatusRequest>
{
    public ActiveUserStatusRequestValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}

public record SuspendUserStatusRequest(Guid User, string? Reason) : IWebRequest
{
    public SuspendUserCommand ToCommand() => new(UserId.From(User), Text.From(Reason!));
}

public class SuspendUserStatusRequestValidator : RequestValidator<SuspendUserStatusRequest>
{
    public SuspendUserStatusRequestValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}

public record BlockUserStatusRequest(Guid User, string Reason) : IWebRequest
{
    public BlockUserCommand ToCommand() => new(UserId.From(User), Text.From(Reason));
}

public class BlockUserStatusRequestValidator : RequestValidator<BlockUserStatusRequest>
{
    public BlockUserStatusRequestValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}