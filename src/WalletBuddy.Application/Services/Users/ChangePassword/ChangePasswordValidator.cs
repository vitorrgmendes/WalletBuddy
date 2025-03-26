using FluentValidation;
using WalletBuddy.Communication.Requests.Users;

namespace WalletBuddy.Application.Services.Users.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
