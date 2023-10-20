using System;
using System.Collections.Generic;

namespace Wordly.Core.Models.Entities;

public sealed class UserTerm : EntityBase<Guid>
{
    public UserTerm(string term, string definition, Guid userId, string[] tags, string description)
        : base(Guid.NewGuid())
    {
        Term = term;
        Definition = definition;
        UserId = userId;
        Tags = tags ?? Array.Empty<string>();
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        Description = description;
    }

    public UserTerm()
    {
    }

    public string Term { get; private set; }
    public string Definition { get; private set; }
    public Guid? ImageBlobId { get; private set; }
    public string ImageUrl { get; private set; }
    public Guid UserId { get; init; }
    public User User { get; init; }
    public string[] Tags { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; private set; }
    public string Description { get; private set; }
    public List<CollectionTerm> CollectionTerms { get; private set; }

    public void SetTerm(string term)
    {
        Term = term;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetDefinition(string definition)
    {
        Definition = definition;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetImageUrlAndBlobId(string imageUrl, Guid blobId)
    {
        ImageUrl = imageUrl;
        ImageBlobId = blobId;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetTags(string[] tags)
    {
        Tags = tags ?? Array.Empty<string>();
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void DeleteImage()
    {
        ImageUrl = null;
        ImageBlobId = null;
    }

    public void SetDescription(string description)
    {
        Description = description;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}