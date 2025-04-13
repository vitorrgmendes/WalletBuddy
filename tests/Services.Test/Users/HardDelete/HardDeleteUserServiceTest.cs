using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Repositories;
using WalletBuddy.Application.Services.Users.HardDelete;
using WalletBuddy.Domain.Entities;

namespace Services.Test.Users.HardDelete;

public class HardDeleteUserServiceTest
{
    [Fact]
    public async Task Success()
    { 
        var user = UserBuilder.Build();

        var service = CreateService(user);

        var exception = await Record.ExceptionAsync(async () => await service.Execute());

        Assert.Null(exception);
    }

    private HardDeleteUser CreateService(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userRepository = new UserRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new HardDeleteUser(loggedUser, userRepository, unitOfWork);
    }
}
