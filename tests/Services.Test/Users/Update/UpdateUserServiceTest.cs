using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using WalletBuddy.Application.Services.Users.Update;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Users.Update;

public class UpdateUserServiceTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var service = CreateService(user);
        var exception = await Record.ExceptionAsync(async () => await service.Execute(request));

        Assert.Null(exception);
        Assert.Equal(user.Name, request.Name);
        Assert.Equal(user.Email, request.Email);
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var service = CreateService(user);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.NAME_REQUIRED, exception.GetErrors());
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var service = CreateService(user, request.Email);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, exception.GetErrors());
    }

    private UpdateUser CreateService(User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new UserRepositoryBuilder().GetUserById(user);

        if (!string.IsNullOrWhiteSpace(email))
            repository.ExistActiveUserWithEmail(email);

        return new UpdateUser(loggedUser, repository.Build(), unitOfWork);
    }
}
