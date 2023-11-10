using System.Collections.Generic;

namespace Wordly.Application.Models.Collection.Test;

public sealed record MatchResponse
{
    public IReadOnlyCollection<string> Terms { get; init; }
    public IReadOnlyCollection<MatchTermResponse> Definitions { get; init; }
}

