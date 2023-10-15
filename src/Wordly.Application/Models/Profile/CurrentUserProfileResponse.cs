using System;

namespace Wordly.Application.Models.Profile;

public sealed record CurrentUserProfileResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}