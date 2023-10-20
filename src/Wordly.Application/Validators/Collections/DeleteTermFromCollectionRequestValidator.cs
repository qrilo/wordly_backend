using FluentValidation;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Validators.Collections;

public class DeleteTermFromCollectionRequestValidator : AbstractValidator<DeleteTermFromCollectionRequest>
{
    public DeleteTermFromCollectionRequestValidator()
    {
        RuleFor(model => model.Ids)
            .NotEmpty();
    }
}