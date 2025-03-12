using FluentValidation;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Exception;

namespace WalletBuddy.Application.Services.Users;

public class CreateUserValidator : AbstractValidator<RequestUserJson>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.NAME_REQUIRED);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_REQUIRED)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator<RequestUserJson>());
    }
}
