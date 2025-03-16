using System.Security.Claims;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Auth.RefreshToken;

public class RefreshToken : IRefreshToken
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshToken(
        IUserRepository userRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseUserRegisteredJson> Execute(RequestRefreshTokenJson request)
    {
        var principal = _accessTokenGenerator.GetTokenPrincipal(request.AccessToken);
        var userIdentifier = principal?.FindFirst(ClaimTypes.Sid)?.Value;

        if (userIdentifier is null)
            throw new InvalidCredentialsException();

        var user = await _userRepository.GetUserByUserIdentifier(Guid.Parse(userIdentifier));
        if (user is null || 
            user.RefreshToken != request.RefreshToken || 
            user.RefreshTokenExpiration < DateTime.UtcNow
            ) 
            throw new InvalidCredentialsException();        

        user.RefreshToken = _accessTokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        //user.LastLogin_At = DateTime.UtcNow;
        _userRepository.Update(user);
        await _unitOfWork.Commit();

        var response = new ResponseUserRegisteredJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user),
            RefreshToken = user.RefreshToken
        };

        return response;
    }    
}
