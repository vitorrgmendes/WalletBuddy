using CommonUtilities.Test.Cryptography;
using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Users.ChangePassword;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Users.ChangePassword;

public class ChangePasswordServiceTest
{
    [Fact]
    public async Task Success()
    { 
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var service = CreateService(user, request.Password);

        var exception = await Record.ExceptionAsync(async () => await service.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Invalid_NewPassword()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var service = CreateService(user, request.Password);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.INVALID_PASSWORD, exception.GetErrors());
    }

    [Fact]
    public async Task Error_Different_CurrentPassword()
    { 
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var service = CreateService(user);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD, exception.GetErrors());
    }

    private ChangePasswordService CreateService(User user, string? password = null)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var userRepository = new UserRepositoryBuilder().GetUserById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new ChangePasswordService(loggedUser, passwordEncrypter, userRepository, unitOfWork);
    }
}
