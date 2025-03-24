using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Application.Services.Users.GetProfile;

public interface IGetUserProfile
{
    Task<ResponseUserProfileJson> Execute();
}
