namespace Kundera.Management.Scopes;

public sealed record ScopeResponse(string Id, string Status)
{
    public IEnumerable<string>? Roles { get; set; }

    public IEnumerable<string>? Services { get; set; }
}