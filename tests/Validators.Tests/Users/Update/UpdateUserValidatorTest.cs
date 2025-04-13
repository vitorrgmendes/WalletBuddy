using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Users.Update;
using WalletBuddy.Exception;

namespace Validators.Tests.Users.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(EmptyStringInLineDataTest))]
    public void Error_Empty_Name(string name)
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_REQUIRED));
    }

    [Theory]
    [ClassData(typeof(EmptyStringInLineDataTest))]
    public void Error_Empty_Email(string email)
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void Error_Invalid_Email()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "test123.com";

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}
