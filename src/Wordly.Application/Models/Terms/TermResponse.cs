using System;

namespace Wordly.Application.Models.Terms;

public class TermResponse
{
    public Guid Id { get; set; }
    public string Term { get; set; }
    public string Definition { get; set; }
    public string[] Tags { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}