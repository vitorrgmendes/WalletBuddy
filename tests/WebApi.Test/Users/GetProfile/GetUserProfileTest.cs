using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.GetProfile;

public class GetUserProfileTest : WalletBuddyClassFixture
{
    private const string URI = "api/user/profile";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;

    public GetUserProfileTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Member.GetToken();
        _userName = customWebApplicationFactory.User_Member.GetName();
        _userEmail = customWebApplicationFactory.User_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    { 
        var result = await DoGet(requestUri: URI, token: _token);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        Assert.Equal(_userName, response.RootElement.GetProperty("name").GetString());
        Assert.Equal(_userEmail, response.RootElement.GetProperty("email").GetString());
    }

    [Fact]
    public async Task Unauthorized()
    {
        var result = await DoGet(requestUri: URI, token: "");

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
