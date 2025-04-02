using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Services.LoggedUser;

namespace WalletBuddy.Application.Services.Users.HardDelete;

public class HardDeleteUser : IHardDeleteUser
{
    public readonly ILoggedUser _loggedUser;
    public readonly IUserRepository _userRepository;
    public readonly IUnitOfWork _unitOfWork;

    public HardDeleteUser(
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

        _userRepository.Delete(loggedUser);
        await _unitOfWork.Commit();
    }
}
