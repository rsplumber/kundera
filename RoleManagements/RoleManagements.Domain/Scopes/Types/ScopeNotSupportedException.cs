namespace RoleManagements.Domain.Scopes.Types;

public class ScopeNotSupportedException : NotSupportedException
{
    public ScopeNotSupportedException(string status) : base($"{status} not supported")
    {
    }
}