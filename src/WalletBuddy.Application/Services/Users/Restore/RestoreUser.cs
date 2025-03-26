using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Users.Restore;

public class RestoreUser : IRestoreUser
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreUser(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var user = await _userRepository.GetUserByIdWithoutFilters(id);

        if (user is null)
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        user.Deleted_At = null;

        _userRepository.Update(user);
        await _unitOfWork.Commit();
    }
}
