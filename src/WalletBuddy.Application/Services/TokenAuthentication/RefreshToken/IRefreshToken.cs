using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.TokenAuthentication.RefreshToken;

public interface IRefreshToken
{
    Task<ResponseUserRegisteredJson> Execute(RequestRefreshTokenJson request);
}
