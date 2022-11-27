using System.Net;
using Core.Domains.Scopes.Types;
using Core.Domains.Services.Types;
using Core.Domains.Users.Types;

namespace Application.Auth.Authorizations;

public record OnAuthorizeEvent(UserId User, ScopeId Scope, ServiceId Service, string Action, IPAddress IpAddress)
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}