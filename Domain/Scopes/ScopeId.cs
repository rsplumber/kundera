using Tes.Domain.Contracts;

namespace Domain.Scopes;

public class ScopeId : CustomType<string, ScopeId>, IIdentity
{
}