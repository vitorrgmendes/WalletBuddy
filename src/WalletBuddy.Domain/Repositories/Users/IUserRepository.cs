using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByUserIdentifier(Guid userIdentifier);
    Task<bool> ExistActiveUserWithEmail(string email);
    Task Register(User user);
    void Update(User user);
}
