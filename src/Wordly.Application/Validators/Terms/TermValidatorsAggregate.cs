using FluentValidation;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Validators.Terms;

public class TermValidatorsAggregate : ITermValidatorsAggregate
{
    public TermValidatorsAggregate(
        IValidator<CreateTermRequest> createTermValidator,
        IValidator<DeleteTermsRequest> deleteTermsRequest,
        IValidator<UpdateTermRequest> updateTermValidator)
    {
        CreateTermValidator = createTermValidator;
        DeleteTermsValidator = deleteTermsRequest;
        UpdateTermValidator = updateTermValidator;
    }
    
    public IValidator<CreateTermRequest> CreateTermValidator { get; }
    public IValidator<DeleteTermsRequest> DeleteTermsValidator { get; }
    public IValidator<UpdateTermRequest> UpdateTermValidator { get; }
}