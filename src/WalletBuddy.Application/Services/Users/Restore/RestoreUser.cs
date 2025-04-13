using FluentValidation.Results;
using WalletBuddy.Domain.Entities;
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

        await Validate(user);

        user.Deleted_At = null;

        _userRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(User user)
    {
        var result = new ValidationResult();

        await _userRepository.ExistActiveUserWithEmail(user.Email).ContinueWith(task =>
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
