using System;
using System.Linq;
using System.Security.Claims;
using KunderaNet.Authorization;
using Microsoft.AspNetCore.Http;

namespace KunderaNet.AspNetCore.Authorization;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool IsAuthenticated() => _accessor.HttpContext!.User
        .Identities
        .Any(identity => identity.IsAuthenticated);

    public ApplicationIdentityUser? User()
    {
        if (!IsAuthenticated()) return null;

        var userId = _accessor.HttpContext!.User.FindFirstValue(KunderaDefaults.UserIdKey);
        return userId is not null ? new ApplicationIdentityUser(Guid.Parse(userId)) : null;
    }
}