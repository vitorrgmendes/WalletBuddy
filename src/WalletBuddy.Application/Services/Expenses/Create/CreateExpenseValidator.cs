using FluentValidation;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Exception;

namespace WalletBuddy.Application.Services.Expenses.Create;

public class CreateExpenseValidator : AbstractValidator<RequestExpenseCreateJson>
{
    public CreateExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(expense => expense.Price).GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_GREATER_THAN_ZERO);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_BE_FUTURE);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}
