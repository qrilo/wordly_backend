using FluentValidation;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Validators.Terms;

public class UpdateTermRequestValidator : AbstractValidator<UpdateTermRequest>
{
    public UpdateTermRequestValidator()
    {
        RuleFor(model => model.Term)
            .NotEmpty()
            .MaximumLength(256);
        
        RuleFor(model => model.Definition)
            .NotEmpty()
            .MaximumLength(256);
        
    }
}