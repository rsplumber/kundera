using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Web.Api.Users;

public record ActiveUserStatusRequest(string? Reason) : IWebRequest
{
    public ActiveUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason!));
}

public record SuspendUserStatusRequest(string? Reason) : IWebRequest
{
    public SuspendUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason!));
}

public record BlockUserStatusRequest(string Reason) : IWebRequest
{
    public BlockUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason));
}