using Moq;
using WalletBuddy.Domain.Repositories.Users;

namespace CommonUtilities.Test.Repositories;

public class UserRepositoryBuilder
{
    private readonly Mock<IUserRepository> _repository;

    public UserRepositoryBuilder()
    {
        _repository = new Mock<IUserRepository>();
    }

    public IUserRepository Build() => _repository.Object;
}
