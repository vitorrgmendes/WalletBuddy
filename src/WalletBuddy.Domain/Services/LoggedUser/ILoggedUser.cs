using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
    Task<User> GetForChanges();
}
