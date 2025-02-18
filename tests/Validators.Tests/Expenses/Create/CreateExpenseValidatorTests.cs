using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Expenses.Create;

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
}
