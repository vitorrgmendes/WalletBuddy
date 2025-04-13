using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Services.LoggedUser;

namespace WalletBuddy.Application.Services.Auth.Logout;

public class LogoutUser : ILogoutUser
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public LogoutUser(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute()
    {
        var user = await _loggedUser.GetForChanges();

        user.RefreshToken = null;
        user.RefreshTokenExpiration = null;
        _userRepository.Update(user);
        await _unitOfWork.Commit();
    }
}
