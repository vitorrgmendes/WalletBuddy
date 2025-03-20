using Bogus;
using CommonUtilities.Test.Cryptography;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Enums;

namespace CommonUtilities.Test.Entities;

public class UserBuilder
{
    public static User Build(string role = Roles.MEMBER, long id = 1)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => id)
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(u => u.Role, _ => role);

        return user;
    }
}
