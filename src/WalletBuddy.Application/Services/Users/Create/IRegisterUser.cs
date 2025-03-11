using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.Users.Create;

public interface IRegisterUser
{
    Task<ResponseUserRegisteredJson> Execute(RequestUserJson request);
}
