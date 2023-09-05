using System;
using System.Linq;
using System.Security.Claims;
using KunderaNet.Authorization;
using Microsoft.AspNetCore.Http;

namespace KunderaNet.AspNetCore.Authorization;

internal sealed class CurrentSessionService : ICurrentSessionService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentSessionService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid? ScopeId()
    {
        if (!IsAuthenticated()) return null;
        var scopeId = _accessor.HttpContext!.User.FindFirstValue(KunderaDefaults.ScopeIdKey);
        return Guid.TryParse(scopeId, out var guid) ? guid : null;
    }

    public Guid? ServiceId()
    {
        if (!IsAuthenticated()) return null;
        var serviceId = _accessor.HttpContext!.User.FindFirstValue(KunderaDefaults.ServiceIdKey);
        return Guid.TryParse(serviceId, out var guid) ? guid : null;
    }

    private bool IsAuthenticated() => _accessor.HttpContext!.User
        .Identities
        .Any(identity => identity.IsAuthenticated);
}