using System;

namespace Wordly.Application.Models.Auth;

public sealed record UserCreatedResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}