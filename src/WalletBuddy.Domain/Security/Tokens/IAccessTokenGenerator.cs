using System.Security.Claims;
using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetTokenPrincipal(string accessToken);
}
