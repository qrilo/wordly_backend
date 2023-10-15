using Microsoft.AspNetCore.Http;

namespace Wordly.Application.Models.Terms;

public sealed record CreateTermRequest
{
    public string Term { get; init; }
    public string Definition { get; init; }
    public string[] Tags { get; init; }
    public IFormFile Image { get; init; }
}