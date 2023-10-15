using FluentValidation;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Validators.Terms;

public class DeleteTermsRequestValidator : AbstractValidator<DeleteTermsRequest>
{
    public DeleteTermsRequestValidator()
    {
        RuleFor(model => model.Ids).NotEmpty();
    }
}