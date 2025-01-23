using FluentValidation;
using WalletBuddy.Communication.Requests.Expenses;

namespace WalletBuddy.Application.Services.Expenses.Create;

public class CreateExpenseValidator : AbstractValidator<RequestExpenseCreateJson>
{
    public CreateExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage("The title is required");
        RuleFor(expense => expense.Price).GreaterThan(0).WithMessage("The price must be greater than zero.");
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be in the future.");
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Payment Type is not valid.");
    }
}
