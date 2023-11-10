using System;

namespace Wordly.Application.Models.Collection.Test;

public sealed record MatchTermResponse
{
    public Guid Id { get; init; }
    public string Definition { get; init; }
}