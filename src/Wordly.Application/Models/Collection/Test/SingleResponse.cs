using System;
using System.Collections.Generic;

namespace Wordly.Application.Models.Collection.Test;

public sealed record SingleResponse
{
    public Guid Id { get; init; }
    public string Question { get; init; }
    public IReadOnlyCollection<string> Answers { get; init; }
}