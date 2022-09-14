using Users.Domain.Users.Exception;
using Users.Domain.Users.Types;

public class TypeTest
{
    [Fact]
    public void username_right_pattern_success()
    {
        var username = Username.From("test");
        Assert.Equal("test", username.Value);
    }
    
    [Fact]
    public void phoneNumber_right_pattern_success()
    {
        var number = PhoneNumber.From("091111111111");
        Assert.Equal("091111111111", number.Value);
    }

    [Fact]
    public void phoneNumber_wrong_pattern_fail()
    {
        Assert.Throws<InvalidPhoneNumberException>(() => { PhoneNumber.From("123"); });

        Assert.Throws<InvalidPhoneNumberException>(() => { PhoneNumber.From("+9891111111111"); });

        Assert.Throws<InvalidPhoneNumberException>(() => { PhoneNumber.From("009891111111111"); });

        Assert.Throws<InvalidPhoneNumberException>(() => { PhoneNumber.From("91111111111"); });
    }

    [Fact]
    public void email_right_pattern_success()
    {
        var email = Email.From("test@test.com");
        Assert.Equal("test@test.com", email.Value);
    }

    [Fact]
    public void email_wrong_pattern_fail()
    {
        Assert.Throws<InvalidEmailException>(() => { Email.From("test"); });

        Assert.Throws<InvalidEmailException>(() => { Email.From("test.com"); });

        Assert.Throws<InvalidEmailException>(() => { Email.From("test@.com"); });

        Assert.Throws<InvalidEmailException>(() => { Email.From("test@"); });

        Assert.Throws<InvalidEmailException>(() => { Email.From("+test@test.com"); });
    }
    
    [Fact]
    public void nationalCode_right_pattern_success()
    {
        var nationalCode = NationalCode.From("1234567891");
        Assert.Equal("1234567891", nationalCode.Value);
    }
    
    [Fact]
    public void nationalCode_wrong_pattern_fail()
    {
        Assert.Throws<InvalidNationalCodeException>(() => { NationalCode.From("1111111111"); });

        Assert.Throws<InvalidNationalCodeException>(() => { NationalCode.From("12345678911"); });

        Assert.Throws<InvalidNationalCodeException>(() => { NationalCode.From("123456789"); });
    }
}