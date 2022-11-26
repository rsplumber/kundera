namespace Core.Domains;

public sealed record Text
{
    private readonly string? _value;

    private Text(string? value)
    {
        _value = value?.ToLower();
    }

    public static Text From(string? value) => new(value);

    public static implicit operator string?(Text? text) => text?.Value;

    public static implicit operator Text?(string? text)
    {
        return text != null ? From(text) : null;
    }

    public string? Value => _value;

    public bool Equals(Text? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return (_value != null ? _value.GetHashCode() : 0);
    }

    public override string ToString()
    {
        return _value ?? string.Empty;
    }
}