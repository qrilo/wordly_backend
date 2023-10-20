using System;

namespace Wordly.Application.Models.Collection;

public sealed record AddTermsToCollectionRequest
{
    public Guid[] Ids { get; init; }
}