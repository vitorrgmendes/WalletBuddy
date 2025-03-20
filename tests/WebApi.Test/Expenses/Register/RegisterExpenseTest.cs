using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Exception;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest : WalletBuddyClassFixture
{
    private const string URI = "api/expenses";

    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var result = await DoPost(requestUri: URI, request: request, token: _token);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        Assert.Equal(request.Title, response.RootElement.GetProperty("title").GetString());
    }    

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPost(requestUri: URI, request: request, token: _token, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Unauthorized()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPost(requestUri: URI, request: request);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
