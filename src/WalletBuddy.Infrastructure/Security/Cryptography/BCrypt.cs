using WalletBuddy.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace WalletBuddy.Infrastructure.Security.Cryptography;

internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);
        return passwordHash;
    }
}
