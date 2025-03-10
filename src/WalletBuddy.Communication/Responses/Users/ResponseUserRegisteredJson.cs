namespace WalletBuddy.Communication.Responses.Users;

public class ResponseUserRegisteredJson
{
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
