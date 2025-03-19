using CommonUtilities.Test.InlineData;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Exception;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Member.GetToken();
        _expenseId = customWebApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{URI}/{_expenseId}", token: _token);

        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

        result = await DoGet(requestUri: $"{URI}/{_expenseId}", token: _token);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{URI}/777", token: _token, culture: culture);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Unauthorized()
    {
        var result = await DoGet(requestUri: $"{URI}/{_expenseId}", token: "");

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
