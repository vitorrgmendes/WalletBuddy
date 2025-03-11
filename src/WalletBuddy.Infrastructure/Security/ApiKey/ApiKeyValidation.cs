using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using WalletBuddy.Domain.Security.ApiKey;

namespace WalletBuddy.Infrastructure.Security.ApiKey;

public class ApiKeyValidation : IApiKeyValidation
{
    private readonly string _apiKey;

    public ApiKeyValidation(string apiKey)
    {
        _apiKey = apiKey;
    }

    public bool IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
            return false;

        return _apiKey!.Equals(userApiKey);
    }
}
