using System;
using System.Collections.Generic;
using System.Linq;
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
            Tags = userTerm.Tags,
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
            Tags = userTerm.Tags,
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
            Tags = userTerm.Tags,
            ImageUrl = userTerm.ImageUrl,
            Description = userTerm.Description,
            CreatedAtUtc = userTerm.CreatedAtUtc
        };
    }

    public IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule)
    {
        return sources?.Select(rule).ToArray();
    }
}