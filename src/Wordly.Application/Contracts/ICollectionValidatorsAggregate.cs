using FluentValidation;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Contracts;

public interface ICollectionValidatorsAggregate
{
      IValidator<AddTermsToCollectionRequest> AddTermsToCollectionValidator { get; }
      IValidator<CreateCollectionRequest> CreateCollectionValidator { get; }
      IValidator<DeleteTermFromCollectionRequest> DeleteTermFromCollectionValidator { get; }
      IValidator<UpdateCollectionRequest> UpdateCollectionValidator { get; }
}