using Authentication.Domain.Types;

namespace Authentication.Domain.Tests;

public class TypeTests
{
    [Fact]
    public void UniqueIdentifierDefaultTypeCreation()
    {
        var uniqueIdentifier = UniqueIdentifier.From("123456");
        Assert.NotNull(uniqueIdentifier);
        Assert.NotEmpty(uniqueIdentifier.Value);
        Assert.NotEmpty(uniqueIdentifier.Type);

        Assert.Equal("123456", uniqueIdentifier.Identifier);
        Assert.Equal("default", uniqueIdentifier.Type);
        Assert.Equal("123456_:_default", uniqueIdentifier.Value);
    }

    [Fact]
    public void UniqueIdentifierCustomTypeCreation()
    {
        var uniqueIdentifier = UniqueIdentifier.From("plumber@gmail.COM", "App");
        Assert.NotNull(uniqueIdentifier);
        Assert.NotEmpty(uniqueIdentifier.Value);
        Assert.NotEmpty(uniqueIdentifier.Type);

        Assert.Equal("plumber@gmail.com", uniqueIdentifier.Identifier);
        Assert.Equal("app", uniqueIdentifier.Type);
        Assert.Equal("plumber@gmail.com_:_app", uniqueIdentifier.Value);
    }

    [Fact]
    public void UniqueIdentifierDefaultTypeEquality()
    {
        var uniqueIdentifier = UniqueIdentifier.From("123456");
        var uniqueIdentifier2 = UniqueIdentifier.From("123456");
        Assert.Equal(uniqueIdentifier, uniqueIdentifier2);
        Assert.True(uniqueIdentifier.Equals(uniqueIdentifier2));

        var uniqueIdentifier3 = UniqueIdentifier.From("1234567");
        Assert.NotEqual(uniqueIdentifier, uniqueIdentifier3);
        Assert.False(uniqueIdentifier.Equals(uniqueIdentifier3));
    }

    [Fact]
    public void UniqueIdentifierCustomTypeEquality()
    {
        var uniqueIdentifier = UniqueIdentifier.From("123456", "app");
        var uniqueIdentifier2 = UniqueIdentifier.From("123456", "otp");
        Assert.NotEqual(uniqueIdentifier, uniqueIdentifier2);

        var uniqueIdentifier3 = UniqueIdentifier.From("123456", "app");
        var uniqueIdentifier4 = UniqueIdentifier.From("1234567", "app");
        Assert.NotEqual(uniqueIdentifier3, uniqueIdentifier4);
        Assert.False(uniqueIdentifier3.Equals(uniqueIdentifier4));
    }

    [Fact]
    public void UniqueIdentifierParse()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => UniqueIdentifier.Parse("asda_:_dsada_:_sada_:_"));

        var uniqueIdentifier = UniqueIdentifier.Parse("123456_:_app");
        Assert.NotNull(uniqueIdentifier);
        Assert.Equal("123456", uniqueIdentifier.Identifier);
        Assert.Equal("app", uniqueIdentifier.Type);
        Assert.Equal("123456_:_app", uniqueIdentifier.Value);

        var uniqueIdentifier2 = UniqueIdentifier.Parse("123456_:_app");
        Assert.Equal(uniqueIdentifier, uniqueIdentifier2);
        Assert.True(uniqueIdentifier.Equals(uniqueIdentifier2));

        var uniqueIdentifier3 = UniqueIdentifier.Parse("123456");
        Assert.Equal("123456", uniqueIdentifier3.Identifier);
        Assert.Equal("default", uniqueIdentifier3.Type);
        Assert.Equal("123456_:_default", uniqueIdentifier3.Value);
    }

    [Fact]
    public void PasswordCreation()
    {
        var password = Password.From("123456");
        Assert.NotNull(password);
        Assert.NotEmpty(password.Value);
        Assert.NotEmpty(password.Salt);
    }

    [Fact]
    public void PasswordEquality()
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var password = Password.From("123456", salt);
        var password2 = Password.From("123456", salt);
        Assert.Equal(password, password2);
        Assert.True(password.Equals(password2));

        var password3 = Password.From("AA123456", salt);
        Assert.NotEqual(password, password3);
        Assert.False(password.Equals(password3));
    }

    [Fact]
    public void PasswordVerify()
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var password = Password.From("123456", salt);
        Assert.True(password.Verify("123456"));
        Assert.False(password.Verify("AA123456"));
    }
}