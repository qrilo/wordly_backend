namespace Wordly.Application.Models.Collection;

public sealed record CreateCollectionRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
}