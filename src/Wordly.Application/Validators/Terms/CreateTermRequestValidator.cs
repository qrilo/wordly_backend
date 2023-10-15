using FluentValidation;
using Wordly.Application.Extensions;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Validators.Terms;

public sealed class CreateTermRequestValidator : AbstractValidator<CreateTermRequest>
{
    public CreateTermRequestValidator()
    {
        RuleFor(model => model.Term)
            .NotEmpty()
            .MaximumLength(256);
        
        RuleFor(model => model.Definition)
            .NotEmpty()
            .MaximumLength(256);
    }
}