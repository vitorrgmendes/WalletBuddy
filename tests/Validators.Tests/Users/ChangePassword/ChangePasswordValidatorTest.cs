using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Users.ChangePassword;
using WalletBuddy.Exception;

namespace Validators.Tests.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    { 
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(InvalidPasswordInLineData))]
    public void Error_Invalid_Password(string newPassword)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}
