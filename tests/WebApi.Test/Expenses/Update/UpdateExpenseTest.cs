using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Exception;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public UpdateExpenseTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Member.GetToken();
        _expenseId = customWebApplicationFactory.Expense_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var response = await DoPut(requestUri: $"{URI}/{_expenseId}",
                                      request: request,
                                      token: _token);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{URI}/{_expenseId}",
                                    request: request,
                                    token: _token,
                                    culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{URI}/777",
                                    request: request,
                                    token: _token,
                                    culture: culture);

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
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{URI}/{_expenseId}",
                                    request: request,
                                    token: "");

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
