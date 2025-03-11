using System.Security.Claims;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.TokenAuthentication.Logout;

public class LogoutUser : ILogoutUser
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUser(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Claim? emailClaim)
    {
        var email = emailClaim?.Value;
        if (email is null)
            throw new InvalidCredentialsException();

        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
            throw new InvalidCredentialsException();

        user.RefreshToken = null;
        user.RefreshTokenExpiration = null;
        _userRepository.Update(user);
        await _unitOfWork.Commit();
    }
}
