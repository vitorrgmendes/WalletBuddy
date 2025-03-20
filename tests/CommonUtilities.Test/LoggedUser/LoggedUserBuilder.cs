using Moq;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Services.LoggedUser;

namespace CommonUtilities.Test.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        return mock.Object;
    }
}
