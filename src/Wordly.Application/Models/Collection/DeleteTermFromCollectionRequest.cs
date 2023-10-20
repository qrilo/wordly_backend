using System;

namespace Wordly.Application.Models.Collection;

public sealed record DeleteTermFromCollectionRequest
{
    public Guid[] Ids { get; init; }
}