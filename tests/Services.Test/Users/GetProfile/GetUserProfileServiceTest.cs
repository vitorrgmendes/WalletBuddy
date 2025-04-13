using CommonUtilities.Test.Entities;
using CommonUtilities.Test.LoggedUser;
using CommonUtilities.Test.Mapper;
using WalletBuddy.Application.Services.Users.GetProfile;
using WalletBuddy.Domain.Entities;

namespace Services.Test.Users.GetProfile;

public class GetUserProfileServiceTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var service = CreateService(user);

        var result = await service.Execute();

        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
    }

    private GetUserProfile CreateService(User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfile(mapper, loggedUser);
    }
}
