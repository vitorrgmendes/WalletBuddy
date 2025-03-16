using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Expenses;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Exception;

namespace Validators.Tests.Expenses.Create;

public class CreateExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(EmptyStringInLineDataTest))]
    public void ErrorTitleEmpty(string title)
    {
        var validator = new ExpenseValidator();
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
        var validator = new ExpenseValidator();
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
        var validator = new ExpenseValidator();
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
        var validator = new ExpenseValidator();
        var request = RequestExpenseCreateJsonBuilder.Build();
        request.Price = price;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Equals(ResourceErrorMessages.PRICE_GREATER_THAN_ZERO));
    }
}
