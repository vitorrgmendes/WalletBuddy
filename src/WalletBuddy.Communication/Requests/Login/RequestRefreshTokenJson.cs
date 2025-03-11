namespace WalletBuddy.Communication.Requests.Login;

public class RequestRefreshTokenJson
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
