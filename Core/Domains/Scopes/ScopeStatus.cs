namespace Core.Domains.Scopes;

public sealed class ScopeStatus : Enumeration
{
    public static readonly ScopeStatus Active = new(1, nameof(Active));
    public static readonly ScopeStatus DeActive = new(2, nameof(DeActive));

    private ScopeStatus(int id, string name) : base(id, name)
    {
    }
}