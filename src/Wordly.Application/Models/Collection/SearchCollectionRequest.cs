namespace Wordly.Application.Models.Collection;

public sealed record SearchCollectionRequest
{
    public string Name { get; init; }
}