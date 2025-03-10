using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Login;

public class LoginUser : ILoginUser
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUser(
        IPasswordEncripter passwordEncripter, 
        IUserRepository userRepository, 
        IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordEncripter = passwordEncripter;
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseUserRegisteredJson> Execute(RequestLoginJson request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

        bool passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        return new ResponseUserRegisteredJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}
