using Bogus;
using WalletBuddy.Communication.Requests.Login;

namespace CommonUtilities.Test.Requests;

public class RequestUserLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
