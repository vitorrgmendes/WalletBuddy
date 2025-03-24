using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.Auth.RefreshToken;

public interface IRefreshToken
{
    Task<ResponseUserLoggedJson> Execute(RequestRefreshTokenJson request);
}
