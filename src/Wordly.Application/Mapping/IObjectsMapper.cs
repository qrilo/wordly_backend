using System;
using System.Collections.Generic;
using Wordly.Application.Models.Profile;
using Wordly.Application.Models.Terms;
using Wordly.Core.Models.Entities;

namespace Wordly.Application.Mapping;

public interface IObjectsMapper
{
    CurrentUserProfileResponse ToCurrentUserProfileResponse(User user);
    TermCreatedResponse ToTermCreatedResponse(UserTerm userTerm);
    TermUpdatedResponse ToTermUpdateResponse(UserTerm userTerm);
    TermResponse ToTermResponse(UserTerm userTerm);

    IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule);
}