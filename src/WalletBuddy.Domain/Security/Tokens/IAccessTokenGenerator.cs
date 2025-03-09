using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string GenerateAccessToken(User user);
}
