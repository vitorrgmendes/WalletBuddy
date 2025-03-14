using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Requests;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WalletBuddy.Exception;

namespace WebApi.Test.Users;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string URI = "api/user/register";

    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();        

        var result = await _httpClient.PostAsJsonAsync(URI, request);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        //var response = await result.Content.ReadFromJsonAsync<ResponseUserRegisteredJson>();

        Assert.Equal(request.Name, response.RootElement.GetProperty("name").GetString());
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("token").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(response.RootElement.GetProperty("refreshToken").GetString()));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        var result = await _httpClient.PostAsJsonAsync(URI, request);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_REQUIRED", new CultureInfo(culture));

        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }
}
