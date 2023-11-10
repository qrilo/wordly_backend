using System;

namespace Wordly.Application.Models.Collection.Test;

public sealed record WrittenResponse
{
    public Guid Id { get; init; }
    public string Question { get; init; }
}