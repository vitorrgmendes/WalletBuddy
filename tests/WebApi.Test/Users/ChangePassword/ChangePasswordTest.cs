using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Exception;

namespace WebApi.Test.Users.ChangePassword;

public class ChangePasswordTest : WalletBuddyClassFixture
{
    private const string URI = "api/user/change-password";
    private const string loginURI = "api/auth/login";

    private readonly string _token;
    private readonly string _password;
    private readonly string _email;

    public ChangePasswordTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Member.GetToken();
        _password = customWebApplicationFactory.User_Member.GetPassword();
        _email = customWebApplicationFactory.User_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var response = await DoPut(requestUri: URI, request: request, token: _token);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost(requestUri: loginURI, request: loginRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        loginRequest.Password = request.NewPassword;

        response = await DoPost(requestUri: loginURI, request: loginRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Different_CurrentPassword(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await DoPut(requestUri: URI, request: request, token: _token, culture: culture);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }
}
