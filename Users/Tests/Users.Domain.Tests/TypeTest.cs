using Users.Domain.Users.Types;

public class TypeTest
{
    [Fact]
    public void username_right_pattern_success()
    {
        var username = Username.From("test");
        Assert.Equal("test", username.Value);
    }
}