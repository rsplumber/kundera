using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Scopes.Types;

public class ScopeId : CustomType<string, ScopeId>, IIdentity
{
}