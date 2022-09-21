using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public record UniqueIdentifier : IIdentity
{
    private const string DefaultType = "default";
    private readonly string _identifier;
    private readonly string _type;

    private UniqueIdentifier(string identifier, string? type)
    {
        _identifier = identifier.ToLower();
        _type = type is not null ? type.ToLower() : DefaultType;
    }

    public static UniqueIdentifier From(string identifier, string? type = null) => new(identifier, type);

    public static UniqueIdentifier Parse(string uniqueIdentifier)
    {
        var split = uniqueIdentifier.Split("_:_");
        if (split.Length is 0 or > 2)
        {
            throw new ArgumentOutOfRangeException();
        }

        var identifier = split.First();
        var type = split.Length == 2 ? split.LastOrDefault() : DefaultType;
        return new(identifier, type);
    }

    public static implicit operator string(UniqueIdentifier uniqueIdentifier) => uniqueIdentifier.Value;

    public string Type => _type;

    public string Identifier => _identifier;

    public string Value => $"{_identifier}_:_{_type}";

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