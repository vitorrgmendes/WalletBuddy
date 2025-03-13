using System.Security.Claims;

namespace WalletBuddy.Application.Services.Auth.Logout;

public interface ILogoutUser
{
    Task Execute(Claim? claim);
}
