using Kundera.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kundera.AspNetCore.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public AuthorizeAttribute()
    {
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (AllowAnonymous()) return;

        var user = GetUserFromContext();

        if (user is null)
        {
            // context.Result = GenerateUnauthorizedResponse();
        }

        bool AllowAnonymous() => context.ActionDescriptor
            .EndpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any();

        ApplicationIdentityUser? GetUserFromContext() => context.HttpContext.Items["User"] as ApplicationIdentityUser;

        // Jsonn GenerateUnauthorizedResponse() => new(new
        // {
        //     code = 401, message = "Unauthorized"
        // }) {StatusCode = StatusCodes.Status401Unauthorized};
    }
}