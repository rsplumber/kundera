using Kite.Web.Requests;
using Managements.Application.Users;
using Managements.Domain;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Web.Api.Users;

public record SuspendUserStatusRequest(string? Reason) : IWebRequest
{
    public SuspendUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason!));
}

public record BlockUserStatusRequest(string Reason) : IWebRequest
{
    public BlockUserCommand ToCommand(Guid userId) => new(UserId.From(userId), Text.From(Reason));
}