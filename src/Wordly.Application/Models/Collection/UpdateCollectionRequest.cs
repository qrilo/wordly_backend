using System;

namespace Wordly.Application.Models.Collection;

public class UpdateCollectionRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
}