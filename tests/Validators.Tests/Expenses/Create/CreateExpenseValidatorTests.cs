using CommonUtilities.Test.Requests;
using System.Runtime.CompilerServices;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Exception;

namespace Validators.Tests.Expenses.Create;

public class CreateExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new CreateExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [MemberData(nameof(GetWrongTitleData))]
    public void ErrorTitleEmpty(string? title)
    {
        var validator = new CreateExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void ErrorDateFuture()
    {
        var validator = new CreateExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_BE_FUTURE));
    }

    [Fact]
    public void ErrorPaymentTypeInvalid()
    {
        var validator = new CreateExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.PaymentType = (PaymentType)55;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(-7)]
    [InlineData(0)]
    public void ErrorPriceInvalid(decimal price)
    {
        var validator = new CreateExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.Price = price;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.PRICE_GREATER_THAN_ZERO));
    }

    public static IEnumerable<object[]> GetWrongTitleData()
    {
        yield return new object[] { "" };
        yield return new object[] { "  " };
        yield return new object[] { null };
    }
}
