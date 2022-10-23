namespace Managements.Domain.Scopes.Types;

public class ScopeStatus
{
    public static readonly ScopeStatus Active = From(nameof(Active));
    public static readonly ScopeStatus DeActive = From(nameof(DeActive));

    private readonly string _value;

    private ScopeStatus(string value)
    {
        _value = value;
        Validate();
    }

    public static ScopeStatus From(string value) => new(value);

    private void Validate()
    {
        if (Value is not (nameof(Active) or nameof(DeActive)))
        {
            throw new ScopeNotSupportedException(Value);
        }
    }

    public string Value => _value;

    public bool Equals(ScopeStatus? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }
}