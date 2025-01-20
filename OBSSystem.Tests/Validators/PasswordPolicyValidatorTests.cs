using OBSSystem.Application.Validators;
using Xunit;

public class PasswordPolicyValidatorTests
{
    [Fact]
    public void Password_ShouldBeInvalid_IfTooShort()
    {
        Assert.False(PasswordPolicyValidator.IsPasswordValid("Ab1!")); // Çok kısa
    }

    [Fact]
    public void Password_ShouldBeInvalid_IfMissingUppercase()
    {
        Assert.False(PasswordPolicyValidator.IsPasswordValid("abcd1234!")); // Büyük harf yok
    }

    [Fact]
    public void Password_ShouldBeInvalid_IfBannedPassword()
    {
        Assert.False(PasswordPolicyValidator.IsPasswordValid("123456")); // Yasaklı şifre
    }

    [Fact]
    public void Password_ShouldBeValid_IfMeetsAllCriteria()
    {
        Assert.True(PasswordPolicyValidator.IsPasswordValid("Abc123!@")); // Geçerli şifre
    }
}
