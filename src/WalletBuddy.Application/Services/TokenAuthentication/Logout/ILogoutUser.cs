using System.Security.Claims;

namespace WalletBuddy.Application.Services.TokenAuthentication.Logout;

public interface ILogoutUser
{
    Task Execute(Claim? claim);
}
