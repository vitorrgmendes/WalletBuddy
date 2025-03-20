using CommonUtilities.Test.InlineData;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Exception;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    { 
        _token = customWebApplicationFactory.User_Member.GetToken();
        _expenseId = customWebApplicationFactory.Expense_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{URI}/{_expenseId}", token: _token);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        Assert.Equal(_expenseId, response.RootElement.GetProperty("id").GetInt64());
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("title").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("description").GetString()));
        Assert.True(response.RootElement.GetProperty("date").GetDateTime() <= DateTime.Today);
        Assert.True(response.RootElement.GetProperty("price").GetDecimal() > 0);
        Assert.True(Enum.IsDefined(typeof(PaymentType), response.RootElement.GetProperty("paymentType").GetInt32()));
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
