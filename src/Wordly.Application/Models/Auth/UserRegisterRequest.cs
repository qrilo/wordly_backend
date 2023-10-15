namespace Wordly.Application.Models.Auth;

public sealed record UserRegisterRequest
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}