using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public record UniqueIdentifier : IIdentity
{
    private const string DefaultType = "default";
    private readonly string _identifier;
    private readonly string _type;

    private UniqueIdentifier(string identifier)
    {
        _identifier = identifier;
        _type = DefaultType;
    }

    private UniqueIdentifier(string identifier, string type)
    {
        _identifier = identifier;
        _type = type;
    }

    public static UniqueIdentifier From(string identifier) => new(identifier);

    public static UniqueIdentifier From(string identifier, string type) => new(identifier, type);

    public static implicit operator string(UniqueIdentifier uniqueIdentifier) => uniqueIdentifier.Value;

    public string Type => _type;

    public string Identifier => _identifier;

    public string Value => $"{_identifier}_{_type}";

    public virtual bool Equals(UniqueIdentifier? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _identifier == other._identifier && _type == other._type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_identifier, _type);
    }

    public override string ToString()
    {
        return Value;
    }
}