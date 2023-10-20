using System;

namespace Wordly.Core.Models.Entities;

public class CollectionTerm : EntityBase<Guid>
{
    public CollectionTerm(Guid userTermId, Guid collectionId)
        : base(Guid.NewGuid())
    {
        TermId = userTermId;
        CollectionId = collectionId;
    }

    private CollectionTerm()
    {
    }

    public Guid CollectionId { get; init; }
    public Collection Collection { get; init; }
    public Guid TermId { get; init; }
    public UserTerm Term { get; init; }
}