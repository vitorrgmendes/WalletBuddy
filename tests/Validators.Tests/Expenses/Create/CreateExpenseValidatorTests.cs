using CommonUtilities.Test.Requests;
using FluentAssertions;
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

        result.IsValid.Should().BeTrue();
    }
}
