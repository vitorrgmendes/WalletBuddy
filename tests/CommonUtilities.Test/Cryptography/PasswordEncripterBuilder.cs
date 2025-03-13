using Moq;
using WalletBuddy.Domain.Security.Cryptography;

namespace CommonUtilities.Test.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    { 
        var mock = new Mock<IPasswordEncripter>();

        mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("Encrypted-Password");

        return mock.Object;
    }
}
