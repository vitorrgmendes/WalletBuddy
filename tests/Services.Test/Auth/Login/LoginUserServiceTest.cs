using CommonUtilities.Test.Cryptography;
using CommonUtilities.Test.Entities;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using CommonUtilities.Test.Token;
using WalletBuddy.Application.Services.Auth.Login;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace Services.Test.Auth.Login;

public class LoginUserServiceTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUserLoginJsonBuilder.Build();
        request.Email = user.Email;

        var service = CreateService(user, request.Password);

        var result = await service.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(result.Name, user.Name);
        Assert.True(!string.IsNullOrWhiteSpace(result.Token));
        Assert.True(!string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task Error_User_Not_FoundAsync()
    {
        var user = UserBuilder.Build();
        var request = RequestUserLoginJsonBuilder.Build();

        var service = CreateService(user, request.Password);

        InvalidLoginException exception = await Assert.ThrowsAsync<InvalidLoginException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.INVALID_LOGIN, exception.GetErrors());
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestUserLoginJsonBuilder.Build();
        request.Email = user.Email;

        var service = CreateService(user);

        InvalidLoginException exception = await Assert.ThrowsAsync<InvalidLoginException>(async () => await service.Execute(request));

        Assert.Single(exception.GetErrors());
        Assert.Contains(ResourceErrorMessages.INVALID_LOGIN, exception.GetErrors());
    }

    private LoginUser CreateService(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var userRepository = new UserRepositoryBuilder().GetUserByEmail(user).Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();

        return new LoginUser(passwordEncrypter, userRepository, accessTokenGenerator, unitOfWork);
    }
}
