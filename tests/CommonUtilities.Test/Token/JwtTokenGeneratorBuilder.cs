using Moq;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Security.Tokens;

namespace CommonUtilities.Test.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>())).Returns("Access-Token");
        mock.Setup(accessTokenGenerator => accessTokenGenerator.GenerateRefreshToken()).Returns("Refresh-Token");

        return mock.Object;
    }
}
