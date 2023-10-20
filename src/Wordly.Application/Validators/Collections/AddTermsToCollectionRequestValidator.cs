using FluentValidation;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Validators.Collections;

public sealed class AddTermsToCollectionRequestValidator : AbstractValidator<AddTermsToCollectionRequest>
{
    public AddTermsToCollectionRequestValidator()
    {
        RuleFor(model => model.Ids)
            .NotEmpty();
    }
}