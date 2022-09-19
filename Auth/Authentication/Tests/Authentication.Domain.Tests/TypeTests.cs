namespace Authentication.Domain.Tests;

public class TypeTests
{
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