using System;

namespace Wordly.Application.Models.Terms;

public sealed record TermUpdatedResponse
{
    public Guid Id { get; set; }
    public string Term { get; set; }
    public string Definition { get; set; }
    public string[] Tags { get; set; }
    public string ImageUrl { get; set; }
}