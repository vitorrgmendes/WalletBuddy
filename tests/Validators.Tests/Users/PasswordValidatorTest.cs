using CommonUtilities.Test.InlineData;
using FluentValidation;
using WalletBuddy.Application.Services.Users;
using WalletBuddy.Communication.Requests.Users;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [ClassData(typeof(InvalidPasswordInLineData))]
    public void Error_Invalid_Password(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        Assert.False(result);
    }
}
