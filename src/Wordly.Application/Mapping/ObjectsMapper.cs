using System;
using System.Collections.Generic;
using System.Linq;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Profile;
using Wordly.Application.Models.Terms;
using Wordly.Core.Models.Entities;

namespace Wordly.Application.Mapping;

public sealed class ObjectsMapper : IObjectsMapper
{
    public CurrentUserProfileResponse ToCurrentUserProfileResponse(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return new CurrentUserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
        };
    }

    public TermCreatedResponse ToTermCreatedResponse(UserTerm userTerm)
    {
        ArgumentNullException.ThrowIfNull(userTerm, nameof(userTerm));

        return new TermCreatedResponse()
        {
            Id = userTerm.Id,
            Term = userTerm.Term,
            Definition = userTerm.Definition,
            Tags = userTerm.GetTags(),
            ImageUrl = userTerm.ImageUrl,
            Description = userTerm.Description,
            CreatedAtUtc = userTerm.CreatedAtUtc
        };
    }

    public TermUpdatedResponse ToTermUpdateResponse(UserTerm userTerm)
    {
        ArgumentNullException.ThrowIfNull(userTerm, nameof(userTerm));

        return new TermUpdatedResponse()
        {
            Id = userTerm.Id,
            Term = userTerm.Term,
            Definition = userTerm.Definition,
            Tags = userTerm.GetTags(),
            ImageUrl = userTerm.ImageUrl,
            Description = userTerm.Description,
            CreatedAtUtc = userTerm.CreatedAtUtc
        };
    }

    public TermResponse ToTermResponse(UserTerm userTerm)
    {
        ArgumentNullException.ThrowIfNull(userTerm, nameof(userTerm));
        
        return new TermResponse()
        {
            Id = userTerm.Id,
            Term = userTerm.Term,
            Definition = userTerm.Definition,
            Tags = userTerm.GetTags(),
            ImageUrl = userTerm.ImageUrl,
            Description = userTerm.Description,
            CreatedAtUtc = userTerm.CreatedAtUtc
        };
    }

    public CollectionResponse ToCollectionResponse(Collection collections)
    {
        ArgumentNullException.ThrowIfNull(collections, nameof(collections));

        return new CollectionResponse()
        {
            Id = collections.Id,
            Name = collections.Name,
            ImageUrl = collections.CollectionTerms?.Count > 0 ? collections.CollectionTerms.First().Term.ImageUrl : null,
            Description = collections.Description,
            CreatedAtUtc = collections.CreatedAtUtc,
            UpdatedAtUtc = collections.UpdatedAtUtc
        };
    }

    public CollectionSummaryResponse ToCollectionSummaryResponse(Collection collection, IReadOnlyCollection<UserTerm> terms)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return new CollectionSummaryResponse()
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            ImageUrl = terms.FirstOrDefault()?.ImageUrl,
            Terms = MapCollection(terms, ToCollectionTermResponse)
        };
    }

    public CollectionInfoResponse ToCollectionInfoResponse(Collection collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return new CollectionInfoResponse()
        {
            Id = collection.Id,
            Name = collection.Name
        };
    }

    private CollectionTermResponse ToCollectionTermResponse(UserTerm term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return new CollectionTermResponse()
        {
            Id = term.Id,
            Term = term.Term,
            Definition = term.Definition,
            ImageUrl = term.ImageUrl
        };
    }

    public IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule)
    {
        return sources?.Select(rule).ToArray();
    }
}