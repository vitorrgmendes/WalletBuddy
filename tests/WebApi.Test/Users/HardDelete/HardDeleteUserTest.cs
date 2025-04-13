using System.Net;

namespace WebApi.Test.Users.HardDelete;

public class HardDeleteUserTest : WalletBuddyClassFixture
{
    private const string URI = "api/user/delete";

    private readonly string _token;

    public HardDeleteUserTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: URI, token: _token);

        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }
}
