using System;

namespace Wordly.Application.Models.Auth;

public sealed record JwtResponse
{
    public string AccessToken { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
}