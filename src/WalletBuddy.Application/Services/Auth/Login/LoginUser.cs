using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Auth.Login;

public class LoginUser : ILoginUser
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUser(
        IPasswordEncripter passwordEncripter, 
        IUserRepository userRepository, 
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _passwordEncripter = passwordEncripter;
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseUserRegisteredJson> Execute(RequestLoginJson request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

        bool passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        var response = new ResponseUserRegisteredJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user),
            RefreshToken = _accessTokenGenerator.GenerateRefreshToken()
        };

        user.RefreshToken = response.RefreshToken;        
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        user.LastLogin_At = DateTime.UtcNow;
        _userRepository.Update(user);
        await _unitOfWork.Commit();

        return response;
    }
}
