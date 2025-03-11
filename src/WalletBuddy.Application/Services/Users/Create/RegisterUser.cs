using AutoMapper;
using FluentValidation.Results;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Users.Create;

public class RegisterUser : IRegisterUser
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUser(
        IMapper mapper, 
        IPasswordEncripter passwordEncripter, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseUserRegisteredJson> Execute(RequestUserJson request)
    {
        await Validate(request);               

        var user = _mapper.Map<User>(request);

        var response = new ResponseUserRegisteredJson
        {
            Name = request.Name,
            Token = _accessTokenGenerator.Generate(user),
            RefreshToken = _accessTokenGenerator.GenerateRefreshToken()
        };

        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        user.Created_At = DateTime.UtcNow;
        user.Updated_At = DateTime.UtcNow;
        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(_accessTokenGenerator.RefreshTokenExpirationDays);
        // user.LastLogin_At = DateTime.UtcNow;

        await _userRepository.Register(user);
        await _unitOfWork.Commit();

        return response;
    }

    private async Task Validate(RequestUserJson request)
    { 
        var result = new CreateUserValidator().Validate(request);

        await _userRepository.ExistActiveUserWithEmail(request.Email).ContinueWith(task =>
        {
            if (task.Result)
            {
                result.Errors.Add(new ValidationFailure("Email", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }
        });

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
