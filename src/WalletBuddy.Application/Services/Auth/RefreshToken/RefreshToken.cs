using System.Security.Claims;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Auth.RefreshToken;

public class RefreshToken : IRefreshToken
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public RefreshToken(
        IUserRepository userRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserLoggedJson> Execute(RequestRefreshTokenJson request)
    {
        var user = await _loggedUser.GetForChanges();

        if (user is null || 
            user.RefreshToken != request.RefreshToken || 
            user.RefreshTokenExpiration < DateTime.UtcNow)
                throw new InvalidCredentialsException();        

        user.RefreshToken = _accessTokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        _userRepository.Update(user);
        await _unitOfWork.Commit();

        var response = new ResponseUserLoggedJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user),
            RefreshToken = user.RefreshToken
        };

        return response;
    }    
}
