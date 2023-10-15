using System.Net.Mail;
using FluentValidation;
using Wordly.Application.Extensions;
using Wordly.Application.Models.Auth;

namespace Wordly.Application.Validators.Auth;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(model => model.Name)
            .IsRequired()
            .MaximumLength(100);

        RuleFor(model => model.Email)
            .IsRequired()
            .Must(email => MailAddress.TryCreate(email, out _))
            .WithMessage("Has invalid format.");

        RuleFor(model => model.Password).ApplyPasswordValidationRules();
    }
}