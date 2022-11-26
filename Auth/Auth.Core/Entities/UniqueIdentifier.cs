namespace Auth.Core.Entities;

public record UniqueIdentifier
{
    private const string DefaultType = "default";
    private readonly string _username;
    private readonly string _type;

    private UniqueIdentifier(string username, string? type = DefaultType)
    {
        _username = username.ToLower();
        _type = type is not null ? type.ToLower() : DefaultType;
    }

    public static UniqueIdentifier From(string username, string? type = DefaultType) => new(username, type);

    public static UniqueIdentifier Parse(string uniqueIdentifier)
    {
        var split = uniqueIdentifier.Split("_:_");
        if (split.Length is 0 or > 2)
        {
            throw new ArgumentOutOfRangeException();
        }

        var username = split.First();
        var type = split.Length == 2 ? split.Last() : DefaultType;

        return new(username, type);
    }

    public static implicit operator string(UniqueIdentifier uniqueIdentifier) => uniqueIdentifier.Value;

    public string Type => _type;

    public string Username => _username;

    public string Value => $"{_username}_:_{_type}";

    public virtual bool Equals(UniqueIdentifier? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return _username == other._username && _type == other._type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_username, _type);
    }

    public override string ToString()
    {
        return Value;
    }
}