using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.TokenAuthentication.Login;

public interface ILoginUser
{
    Task<ResponseUserRegisteredJson> Execute(RequestLoginJson request);
}
