using FluentValidation;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Validators.Collections;

public class UpdateCollectionRequestValidator : AbstractValidator<UpdateCollectionRequest>
{
    public UpdateCollectionRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty();
    }
}