using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task Register(User user);
}
