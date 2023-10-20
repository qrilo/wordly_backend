using System;

namespace Wordly.Application.Models.Collection;

public record CollectionInfoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}