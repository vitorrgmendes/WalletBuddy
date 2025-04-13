using Bogus;
using WalletBuddy.Communication.Requests.Users;

namespace CommonUtilities.Test.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    { 
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
