using System;
using System.Collections.Generic;

namespace Wordly.Core.Models.Entities;

public sealed class Collection : EntityBase<Guid>
{
    public Collection(string name, string description, Guid authorId)
    :base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        AuthorId = authorId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private Collection()
    {
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid AuthorId { get; init; }
    public User Author { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; private set; }
    public List<CollectionTerm> CollectionTerms { get; private set; }

    public void SetName(string name)
    { 
        Name = name;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetDescription(string description)
    {
        Description = description;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}