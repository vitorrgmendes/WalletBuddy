using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Services.LoggedUser;

namespace WalletBuddy.Application.Services.Users.SoftDelete;

public class SoftDeleteUser : ISoftDeleteUser
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteUser(
        ILoggedUser loggedUser,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.GetForChanges();

        loggedUser.Deleted_At = DateTime.UtcNow;
        loggedUser.RefreshToken = null;
        loggedUser.RefreshTokenExpiration = null;

        _userRepository.Update(loggedUser);
        await _unitOfWork.Commit();
    }
}
