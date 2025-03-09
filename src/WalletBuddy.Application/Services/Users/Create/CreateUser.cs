using AutoMapper;
using FluentValidation.Results;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Users.Create;

public class CreateUser : ICreateUser
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork; 

    public CreateUser(
        IMapper mapper, 
        IPasswordEncripter passwordEncripter, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseUserCreatedJson> Execute(RequestUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<User>(request);

        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        user.Created_At = DateTime.UtcNow;
        user.Updated_At = DateTime.UtcNow;

        await _userRepository.Register(user);
        await _unitOfWork.Commit();

        return new ResponseUserCreatedJson
        {
            Name = user.Name,
            Token = string.Empty
        };
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
