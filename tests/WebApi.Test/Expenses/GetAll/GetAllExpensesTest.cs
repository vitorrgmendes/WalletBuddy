using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses";

    private readonly string _token;

    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    { 
        var result = await DoGet(requestUri: URI, token: _token);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var expenses = response.RootElement.GetProperty("expenses").EnumerateArray();

        Assert.True(expenses.Any());
    }

    [Fact]
    public async Task Unauthorized()
    {
        var result = await DoGet(requestUri: URI, token: "");

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
