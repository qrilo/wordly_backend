using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wordly.Core.Models.Entities;

namespace Wordly.DataAccess.EntityConfigurations;

public sealed class UserTermsConfiguration : EntityConfigurationBase<UserTerm, Guid>
{
    public override void Configure(EntityTypeBuilder<UserTerm> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.Term).IsRequired();
        builder.Property(entity => entity.Definition).IsRequired();
        builder.Property(entity => entity.UserId).IsRequired();
        builder.Property(entity => entity.CreatedAtUtc).IsRequired();
        builder.Property(entity => entity.UpdatedAtUtc).IsRequired();

        builder.Property(entity => entity.Tags)
            .HasConversion(tags => SerializeTags(tags),
                json => DeserializeTags(json));

        builder.HasOne(entity => entity.User)
            .WithMany()
            .HasForeignKey(entity => entity.UserId);

        builder.HasMany(entity => entity.CollectionTerms)
            .WithOne(entity => entity.Term)
            .HasForeignKey(entity => entity.TermId);
    }
    private static string SerializeTags(string[] tags)
    {
        return JsonSerializer.Serialize(tags);
    }
    
    private static string[] DeserializeTags(string json)
    {
        return JsonSerializer.Deserialize<string[]>(json);
    }
}