using CommonUtilities.Test.Requests;
using FluentValidation;
using WalletBuddy.Application.Services.Users;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Exception;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [MemberData(nameof(GetWrongPasswordData))]
    public void Error_Invalid_Password(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        Assert.False(result);
    }

    public static IEnumerable<object[]> GetWrongPasswordData()
    {
        yield return new object[] { "" };
        yield return new object[] { "  " };
        yield return new object[] { null };
        yield return new object[] { "a" };
        yield return new object[] { "aa" };
        yield return new object[] { "aaa" };
        yield return new object[] { "aaaa" };
        yield return new object[] { "aaaaa" };
        yield return new object[] { "aaaaaa" };
        yield return new object[] { "aaaaaaa" };
        yield return new object[] { "test123!-" };
        yield return new object[] { "TEST123-!SA" };
        yield return new object[] { "TeSt!@-Ab" };        
        yield return new object[] { "Test123Sa4" };        
    }
}
