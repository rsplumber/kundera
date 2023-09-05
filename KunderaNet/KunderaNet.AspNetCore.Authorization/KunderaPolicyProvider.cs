using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KunderaNet.AspNetCore.Authorization;

public class KunderaPolicyProvider : IAuthorizationPolicyProvider
{
    private static readonly AuthorizationPolicy KunderaPolicy = new(new[]
    {
        new ClaimsAuthorizationRequirement("default", Enumerable.Empty<string>())
    }, new[]
    {
        KunderaDefaults.Scheme
    });

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return Task.FromResult(KunderaPolicy)!;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(KunderaPolicy);
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return GetDefaultPolicyAsync()!;
    }
}