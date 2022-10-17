using Kite.CustomType;

namespace Managements.Domain.Scopes.Types;

public class ScopeStatus : CustomType<string, ScopeStatus>
{
    public static readonly ScopeStatus Active = From(nameof(Active));
    public static readonly ScopeStatus DeActive = From(nameof(DeActive));

    protected override void Validate()
    {
        if (Value is not (nameof(Active) or nameof(DeActive)))
        {
            throw new ScopeNotSupportedException(Value);
        }

        base.Validate();
    }
}