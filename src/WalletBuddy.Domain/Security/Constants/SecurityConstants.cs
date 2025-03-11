namespace WalletBuddy.Domain.Security.Constants;

public static class SecurityConstants
{
    public const string API_KEY_HEADER_NAME = "X-API-Key";
    public const string API_KEY_PATH_NAME = "Settings:ApiKey";

    public const string REFRESH_TOKEN_EXPIRATION_PATH = "Settings:Jwt:RefreshTokenExpiresDays";
    public const string JWT_TOKEN_EXPIRATION_PATH = "Settings:Jwt:ExpiresMinutes";
    public const string JWT_SIGNINGKEY_PATH = "Settings:Jwt:SigningKey";
}
