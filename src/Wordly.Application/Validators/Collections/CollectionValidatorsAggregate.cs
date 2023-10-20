using FluentValidation;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Collection;

namespace Wordly.Application.Validators.Collections;

public class CollectionValidatorsAggregate : ICollectionValidatorsAggregate
{
    public CollectionValidatorsAggregate(
        IValidator<AddTermsToCollectionRequest> addTermsToCollectionValidator,
        IValidator<CreateCollectionRequest> createCollectionValidator,
        IValidator<DeleteTermFromCollectionRequest> deleteTermFromCollectionValidator,
        IValidator<UpdateCollectionRequest> updateCollectionValidator)
    {
        AddTermsToCollectionValidator = addTermsToCollectionValidator;
        CreateCollectionValidator = createCollectionValidator;
        DeleteTermFromCollectionValidator = deleteTermFromCollectionValidator;
        UpdateCollectionValidator = updateCollectionValidator;
    }
    
    public IValidator<AddTermsToCollectionRequest> AddTermsToCollectionValidator { get; }
    public IValidator<CreateCollectionRequest> CreateCollectionValidator { get; }
    public IValidator<DeleteTermFromCollectionRequest> DeleteTermFromCollectionValidator { get; }
    public IValidator<UpdateCollectionRequest> UpdateCollectionValidator { get; }
}