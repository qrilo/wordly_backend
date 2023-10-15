using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wordly.Core.Models.Entities;

namespace Wordly.DataAccess.EntityConfigurations;

public sealed class UsersConfiguration : EntityConfigurationBase<User, Guid>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.Name).IsRequired();
        builder.Property(entity => entity.Email).IsRequired();
        builder.Property(entity => entity.PasswordHash).IsRequired();
    }
}