using FluentValidation.Results;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Users;
using WalletBuddy.Domain.Security.Cryptography;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Users.ChangePassword;

public class ChangePasswordService : IChangePassword
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private IPasswordEncrypter _passwordEncrypter;

    public ChangePasswordService(
        ILoggedUser loggedUser,
        IPasswordEncrypter passwordEncrypter,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(loggedUser, request);

        var user = await _userRepository.GetUserById(loggedUser.Id);
        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);
        user.Updated_At = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(User loggedUser, RequestChangePasswordJson request)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMatch = _passwordEncrypter.Verify(request.Password, loggedUser.Password);

        if (!passwordMatch)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
