using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Exception;

namespace WebApi.Test.Auth.Login;

public class LoginTest : WalletBuddyClassFixture
{
    private const string URI = "api/auth/login";

    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public LoginTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _email = customWebApplicationFactory.GetEmail();
        _name = customWebApplicationFactory.GetName();
        _password = customWebApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(requestUri: URI, request: request);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        Assert.Equal(_name, response.RootElement.GetProperty("name").GetString());
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("token").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("refreshToken").GetString()));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Login(string culture)
    {
        var request = RequestUserLoginJsonBuilder.Build();

        var result = await DoPost(requestUri: URI, request: request, culture: culture);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVALID_LOGIN", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }
}
