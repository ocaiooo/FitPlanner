using FitPlanner.Application.SharedValidators;
using FitPlanner.Communication.Requests;
using FluentValidation;

namespace FitPlanner.Application.UseCases.User.ChangePassword;

public class ChangeUserPasswordValidator : AbstractValidator<RequestChangeUserPasswordJson>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangeUserPasswordJson>());
    }
}