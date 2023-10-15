using System;

namespace Wordly.Core.Models.Entities;

public sealed class User : EntityBase<Guid>
{
    public User(string name, string email, string passwordHash)
        : base(Guid.NewGuid())
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    private User()
    {
    }

    public string Name { get; set; }
    public string Email { get; }
    public string PasswordHash { get; }
}