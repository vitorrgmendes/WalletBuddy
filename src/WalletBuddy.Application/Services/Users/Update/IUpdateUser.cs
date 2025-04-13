using WalletBuddy.Communication.Requests.Users;

namespace WalletBuddy.Application.Services.Users.Update;

public interface IUpdateUser
{
    Task Execute(RequestUpdateUserJson request);
}
