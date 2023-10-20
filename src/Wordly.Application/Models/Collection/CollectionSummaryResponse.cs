using System;
using System.Collections.Generic;

namespace Wordly.Application.Models.Collection;

public sealed record CollectionSummaryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IReadOnlyCollection<CollectionTermResponse> Terms { get; set; }
}