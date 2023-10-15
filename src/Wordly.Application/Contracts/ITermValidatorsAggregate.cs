using FluentValidation;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Contracts;

public interface ITermValidatorsAggregate
{
    public IValidator<CreateTermRequest> CreateTermValidator { get; }
    public IValidator<DeleteTermsRequest> DeleteTermsValidator { get; }
    public IValidator<UpdateTermRequest> UpdateTermValidator { get; }
}