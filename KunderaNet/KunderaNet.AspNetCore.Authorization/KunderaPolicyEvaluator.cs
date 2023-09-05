using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace KunderaNet.AspNetCore.Authorization;

public class KunderaPolicyEvaluator : PolicyEvaluator
{
    public KunderaPolicyEvaluator(IAuthorizationService authorization) : base(authorization)
    {
    }

    public override Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
    {
        if (!authenticationResult.Succeeded)
        {
            return Task.FromResult(PolicyAuthorizationResult.Forbid());
        }

        var userIdClaim = authenticationResult.Ticket.Principal.FindFirst(KunderaDefaults.UserIdKey);

        if (userIdClaim is null)
        {
            return Task.FromResult(PolicyAuthorizationResult.Forbid());
        }

        var claims = new List<Claim>
        {
            userIdClaim
        };
        var scopeIdClaim = authenticationResult.Ticket.Principal.FindFirst(KunderaDefaults.ScopeIdKey);
        if (scopeIdClaim is not null) claims.Add(scopeIdClaim);
        var serviceIdClaim = authenticationResult.Ticket.Principal.FindFirst(KunderaDefaults.ServiceIdKey);
        if (serviceIdClaim is not null) claims.Add(serviceIdClaim);
        var claimsIdentity = new ClaimsIdentity(claims, KunderaDefaults.AuthenticationType);
        context.User.AddIdentity(claimsIdentity);
        return Task.FromResult(PolicyAuthorizationResult.Success());
    }
}