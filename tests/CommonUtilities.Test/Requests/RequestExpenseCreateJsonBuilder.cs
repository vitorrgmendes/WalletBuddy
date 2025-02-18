using Bogus;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Communication.Requests.Expenses;

namespace CommonUtilities.Test.Requests;

public class RequestExpenseCreateJsonBuilder
{
    public static RequestExpenseCreateJson Build()
    {
        return new Faker<RequestExpenseCreateJson>()
                .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
                .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(r => r.Date, faker => faker.Date.Past())
                .RuleFor(r => r.Price, faker => faker.Random.Decimal(min:0, max:2000))
                .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>());
    }
}
