using Moq;
using WalletBuddy.Domain.Repositories;

namespace CommonUtilities.Test.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}
