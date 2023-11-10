namespace Wordly.Application.Models.Collection.Test;

public sealed record TestRequest
{
    public bool Written { get; init; }
    public bool Single { get; init; }
    public bool Match { get; init; }
    public int Quantity { get; init; }
}