using FluentValidation;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Validators.Collections;

public class CreateCollectionRequestValidator : AbstractValidator<CreateCollectionRequest>
{
    public CreateCollectionRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty();
    }
}