using Moq;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories.Users;

namespace CommonUtilities.Test.Repositories;

public class UserRepositoryBuilder
{
    private readonly Mock<IUserRepository> _repository;

    public UserRepositoryBuilder()
    {
        _repository = new Mock<IUserRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(userRepository => userRepository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserRepository Build() => _repository.Object;
}
