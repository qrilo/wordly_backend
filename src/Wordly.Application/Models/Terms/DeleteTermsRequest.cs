using System;

namespace Wordly.Application.Models.Terms;

public sealed record DeleteTermsRequest
{
    public Guid[] Ids { get; init; }
}