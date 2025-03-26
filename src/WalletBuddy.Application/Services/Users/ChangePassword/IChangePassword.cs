using WalletBuddy.Communication.Requests.Users;

namespace WalletBuddy.Application.Services.Users.ChangePassword;

public interface IChangePassword
{
    Task Execute(RequestChangePasswordJson request);
}
