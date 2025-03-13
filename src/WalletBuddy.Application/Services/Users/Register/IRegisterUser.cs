using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.Users.Register;

public interface IRegisterUser
{
    Task<ResponseUserRegisteredJson> Execute(RequestUserJson request);
}
