namespace WalletBuddy.Domain.Security.ApiKey;

public interface IApiKeyValidation
{
    bool IsValidApiKey(string userApiKey);
}
